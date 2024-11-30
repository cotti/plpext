using System.Collections.Generic;
using System.Threading.Tasks;
using Plpext.UI.ViewModels;

namespace Plpext.UI.Services;

public interface IFileLoaderService
{
    Task<int> GetFileCountAsync(string filePath);
    IAsyncEnumerable<AudioPlayerViewModel> LoadFilesAsync();
}