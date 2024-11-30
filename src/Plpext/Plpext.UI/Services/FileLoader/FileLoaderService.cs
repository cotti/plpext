using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plpext.Core.AudioPlayer;
using Plpext.Core.Interfaces;
using Plpext.UI.ViewModels;

namespace Plpext.UI.Services.FileLoader;

public class FileLoaderService : IFileLoaderService
{
    private readonly IPackExtractor _packExtractor;
    private readonly IAudioConverter _audioConverter;
    private IEnumerable<ReadOnlyMemory<byte>> _currentFile = null!;

    public FileLoaderService(IPackExtractor packExtractor, IAudioConverter audioConverter)
    {
        _packExtractor = packExtractor;
        _audioConverter = audioConverter;
    }

    public async Task<int> GetFileCountAsync(string filePath)
    {
        _currentFile = await _packExtractor.GetFileListAsync(filePath, default);
        return _currentFile?.Count() ?? 0;
    }

    public async IAsyncEnumerable<AudioPlayerViewModel> LoadFilesAsync()
    {
        foreach (var file in _currentFile)
        {
            var audioFile = await _audioConverter.ConvertAudioAsync(file, default);
            yield return new AudioPlayerViewModel(new AudioPlayer(), audioFile);
        }
    }
}