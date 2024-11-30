namespace Plpext.Core.Interfaces;

public interface IPackExtractor
{
    Task<IEnumerable<ReadOnlyMemory<byte>>> GetFileListAsync(string filePath, CancellationToken cancellationToken);
}
