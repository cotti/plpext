using OpenTK.Audio.OpenAL;
using Plpext.Core.Interfaces;
using Plpext.Core.Models;

namespace Plpext.Core.AudioPlayer;

public sealed class AudioPlayer : IAudioPlayer, IDisposable
{
    public event EventHandler<PlaybackStartedEventArgs>? OnPlaybackStarted;
    public event EventHandler<PlaybackStoppedEventArgs>? OnPlaybackStopped;
    public event EventHandler<PlaybackProgress>? OnProgressUpdated;

    private readonly object _lock = new();
    private CancellationTokenSource? _playbackCts;
    private Task? _playbackTask;
    private int? _currentSourceId;
    private int? _currentBufferId;
    private AudioFile? _audioFile;

    public PlaybackState State { get; private set; } = PlaybackState.Stopped;

    public async Task<bool> InitAudioPlayerAsync(AudioFile input, bool autoStart, CancellationToken cancellationToken)
    {
        _audioFile = input;

        _playbackCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        try
        {
            int bufferId = AL.GenBuffer();
            int sourceId = AL.GenSource();

            _currentBufferId = bufferId;
            _currentSourceId = sourceId;

            AL.BufferData(bufferId, ALFormat.Mono16, input.Data.Span, input.Frequency);
            AL.Source(sourceId, ALSourcei.Buffer, bufferId);
            if (autoStart)
                return await Start();
            return true;
        }
        catch (Exception)
        {
            await CleanupPlaybackResources();
            return false;
        }
    }

    private async Task MonitorPlaybackAsync(CancellationToken cancellationToken)
    {
        if (_audioFile == null || !_currentSourceId.HasValue)
            return;

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var sourceState = (ALSourceState)AL.GetSource(_currentSourceId.Value, ALGetSourcei.SourceState);
                if (sourceState == ALSourceState.Stopped)
                    break;
                if (State != PlaybackState.Paused)
                {
                    AL.GetSource(_currentSourceId.Value, ALGetSourcei.ByteOffset, out int bytesPlayed);
                    var currentPosition = TimeSpan.FromSeconds(
                        ((double)(_audioFile.Data.Length - (_audioFile.Data.Length - bytesPlayed)) / 2) / _audioFile.Frequency
                    );

                    OnProgressUpdated?.Invoke(this, new()
                    {
                        CurrentPosition = currentPosition,
                        TotalDuration = _audioFile.Duration
                    });
                }
                await Task.Delay(100, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            //An operation cancelled here is expected behavior.
        }
        catch (Exception)
        {
            //There is little to be done if we get any other exception during monitoring. Finish up playback monitoring.
        }
        finally
        {
            OnProgressUpdated?.Invoke(this, new()
            {
                CurrentPosition = _audioFile.Duration,
                TotalDuration = _audioFile.Duration
            });
            Stop();
        }
    }

    public Task<bool> Start()
    {
        lock (_lock)
        {
            //If we try and start/resume playback without a proper source id or audio file, we can't go on.
            if (_currentSourceId == null || _audioFile == null)
            {
                return Task.FromResult(false);
            }

            try
            {
                if (State == PlaybackState.Playing)
                    return Task.FromResult(false);
                if (State == PlaybackState.Paused)
                {
                    Resume();
                    return Task.FromResult(true);
                }
                if (_playbackTask != null)
                {
                    _playbackCts?.Cancel();
                    _playbackTask = null;
                }

                if (_playbackCts == null || _playbackCts.IsCancellationRequested)
                {
                    _playbackCts = new CancellationTokenSource();
                }

                State = PlaybackState.Playing;
                OnPlaybackStarted?.Invoke(this, new PlaybackStartedEventArgs { AudioFile = _audioFile });

                AL.SourceRewind(_currentSourceId.Value);
                AL.SourcePlay(_currentSourceId.Value);
                _playbackTask = Task.Run(() => MonitorPlaybackAsync(_playbackCts.Token));
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
    }

    public void Resume()
    {
        lock (_lock)
        {
            if (State == PlaybackState.Paused && _currentSourceId.HasValue)
            {
                AL.SourcePlay(_currentSourceId.Value);
                State = PlaybackState.Playing;
            }
        }
    }
    public void Pause()
    {
        lock (_lock)
        {
            if (State == PlaybackState.Playing && _currentSourceId.HasValue)
            {
                AL.SourcePause(_currentSourceId.Value);
                State = PlaybackState.Paused;
            }
        }
    }

    public void Stop()
    {
        lock (_lock)
        {
            _playbackCts?.Cancel();

            if (_currentSourceId.HasValue)
            {
                AL.SourceStop(_currentSourceId.Value);
            }

            _playbackCts?.Cancel();
            State = PlaybackState.Stopped;
        }
        if(_audioFile is not null)
            OnPlaybackStopped?.Invoke(this, new() { AudioFile = _audioFile });
    }

    private Task CleanupPlaybackResources()
    {
        try
        {
            if (_currentSourceId.HasValue)
            {
                AL.DeleteSource(_currentSourceId.Value);
                _currentSourceId = null;
            }

            if (_currentBufferId.HasValue)
            {
                AL.DeleteBuffer(_currentBufferId.Value);
                _currentBufferId = null;
            }
        }
        catch (Exception)
        {
            //Things might be pretty broken at this point if this exception pops.
        }
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Stop();
        CleanupPlaybackResources().GetAwaiter().GetResult();
        _playbackCts?.Dispose();
    }
}