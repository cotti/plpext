using Plpext.Core.Models;

namespace Plpext.Core.Interfaces;
public interface IAudioConverter
{
    Task<AudioFile> ConvertAudioAsync(ReadOnlyMemory<byte> mp3Input, CancellationToken cancellationToken);
}
