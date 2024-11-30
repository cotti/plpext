using Plpext.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plpext.Core.Interfaces;

public interface IAudioPlayer
{
    Task<bool> InitAudioPlayerAsync(AudioFile input, bool autoStart, CancellationToken cancellationToken);
    void Resume();
    void Pause();
    void Stop();
    Task<bool> Start();
    PlaybackState State { get; }

    event EventHandler<PlaybackStartedEventArgs> OnPlaybackStarted;
    event EventHandler<PlaybackStoppedEventArgs> OnPlaybackStopped;
    event EventHandler<PlaybackProgress> OnProgressUpdated;
}
