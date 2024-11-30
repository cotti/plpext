using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace Plpext.UI.Services.PlatformStorage
{
    public class PlatformStorageService : IPlatformStorageService
    {
        public async Task<string> GetOriginFilePath()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                return string.Empty;

            var filePath = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                AllowMultiple = false,
                Title = "Select Plus Library Pack",
                FileTypeFilter = [new ("Plus Library Pack"){Patterns = ["*.plp"]}],
            });

            return filePath.Any() ? filePath[0].Path.AbsolutePath : string.Empty;
        }
        
        public async Task<string> GetTargetFolderPath()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                return string.Empty;

            var tentativeFolder = await provider.TryGetWellKnownFolderAsync(Avalonia.Platform.Storage.WellKnownFolder.Music);

            var outputPath = await provider.OpenFolderPickerAsync(new Avalonia.Platform.Storage.FolderPickerOpenOptions()
            {
                AllowMultiple = false,
                Title = "Select output folder",
                SuggestedStartLocation = tentativeFolder
            });

            return outputPath.Any() ? outputPath[0].Path.AbsolutePath : string.Empty;
        }
    }
}