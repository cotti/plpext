using Plpext.Core.Models;

namespace Plpext.Core.Interfaces;

public interface IFileStorage
{
    Task SaveFilesAsync(IEnumerable<MP3File> files, string targetPath);
}
