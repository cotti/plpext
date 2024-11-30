using CommunityToolkit.Mvvm.ComponentModel;
using Plpext.Core.AudioPlayer;
using Plpext.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace Plpext.UI.ViewModels
{
    public partial class AudioPlayerViewModel : ViewModelBase, IDisposable
    {
        private readonly AudioPlayer _audioPlayer = null!;
        private readonly AudioFile _audioFile = null!;
        private bool _firstExecution = true;
        
        public AudioFile AudioFile
        {
            get { return _audioFile; }
        }

        public AudioPlayerViewModel()
        {
        }

        public AudioPlayerViewModel(AudioPlayer audioPlayer, AudioFile audioFile)
        {
            _audioPlayer = audioPlayer;
            _audioFile = audioFile;
            CurrentDuration = "0:00";
            Name = _audioFile.Name;
            TotalDuration = $"{_audioFile.Duration:m\\:ss}";
            audioPlayer.OnProgressUpdated += OnProgressUpdated;
            audioPlayer.OnPlaybackStopped += OnPlaybackStopped;
        }

        private void OnPlaybackStopped(object? sender, PlaybackStoppedEventArgs e)
        {
            IsPlaying = false;
            PlaybackState = PlaybackState.Stopped;
            CurrentDuration = "0:00";
        }

        private void OnProgressUpdated(object? sender, PlaybackProgress e)
        {
            Progress = e.ProgressPercentage;
            CurrentDuration = $"{e.CurrentPosition:m\\:ss}";
        }

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private bool _isPlaying;

        [ObservableProperty]
        private string _totalDuration = null!;

        [ObservableProperty]
        private string _currentDuration = null!;

        [ObservableProperty]
        private string _name = null!;

        [ObservableProperty] private PlaybackState _playbackState = PlaybackState.Stopped;
        
        [ObservableProperty]
        private double _progress;

        [RelayCommand]
        private async Task Play()
        {
            if (IsPlaying)
            {
                if (PlaybackState == PlaybackState.Playing)
                {
                    _audioPlayer.Pause();
                    PlaybackState = PlaybackState.Paused;
                }
                else if (PlaybackState == PlaybackState.Paused)
                {
                    _audioPlayer.Resume();
                    PlaybackState = PlaybackState.Playing;
                }

                return;
            }

            IsPlaying = true;
            if (_firstExecution)
            {
                _firstExecution = false;
                await _audioPlayer.InitAudioPlayerAsync(_audioFile, true, default);
            }
            else
                await _audioPlayer.Start();
            PlaybackState = PlaybackState.Playing;
        }

        [RelayCommand]
        private Task Stop()
        {
            IsPlaying = false;
            _audioPlayer.Stop();
            PlaybackState = PlaybackState.Stopped;
            return Task.CompletedTask;
        }


        public void Dispose()
        {
            _audioPlayer.OnPlaybackStopped -= OnPlaybackStopped;
            _audioPlayer.OnProgressUpdated -= OnProgressUpdated;
            _audioPlayer.Dispose();
        }
    }
}