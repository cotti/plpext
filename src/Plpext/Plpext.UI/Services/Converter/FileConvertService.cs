using System.Collections.Generic;
using System.Threading.Tasks;
using Plpext.Core.Interfaces;
using Plpext.Core.Models;

namespace Plpext.UI.Services.Converter;

public class FileConvertService : IConvertService
{
    private readonly IMP3Parser _mp3Parser;
    private readonly IFileStorage _fileStorage;

    public FileConvertService(IMP3Parser mp3Parser, IFileStorage fileStorage)
    {
        _mp3Parser = mp3Parser;
        _fileStorage = fileStorage;
    }

    public async Task ConvertFilesAsync(IEnumerable<AudioFile> files, string outputFolder)
    {
        var mp3Files = new List<MP3File>();
        foreach (var file in files)
        {
            mp3Files.Add(await _mp3Parser.ParseIntoMP3(file.MP3Data, default));
        }
        await _fileStorage.SaveFilesAsync(mp3Files, outputFolder);
    }
}