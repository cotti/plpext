using System.Threading.Tasks;

namespace Plpext.UI.Services;

public interface IPlatformStorageService
{
    Task<string> GetOriginFilePath();
    Task<string> GetTargetFolderPath();
}