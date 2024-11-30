namespace Plpext.Core.Models;

public class PlaybackStoppedEventArgs : EventArgs
{
    public required AudioFile AudioFile { get; init; }
}

public class PlaybackStartedEventArgs : EventArgs
{
    public required AudioFile AudioFile { get; init; }
}
