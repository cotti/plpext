namespace Plpext.Core.Models;

public record PlaybackProgress
{
    public TimeSpan CurrentPosition { get; init; }
    public TimeSpan TotalDuration { get; init; }
    public double ProgressPercentage => TotalDuration.TotalSeconds > 0 ? (CurrentPosition.TotalSeconds / TotalDuration.TotalSeconds) * 100 : 0;
}