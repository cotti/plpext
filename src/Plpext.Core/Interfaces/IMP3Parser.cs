using Plpext.Core.Models;

namespace Plpext.Core.Interfaces;

public interface IMP3Parser
{
    Task<MP3File> ParseIntoMP3(ReadOnlyMemory<byte> data, CancellationToken cancellationToken);
}
