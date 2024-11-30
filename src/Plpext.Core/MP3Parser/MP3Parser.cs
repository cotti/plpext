using Plpext.Core.Interfaces;
using Plpext.Core.Models;
using System.Text;

namespace Plpext.Core.MP3Parser;

public class MP3Parser : IMP3Parser
{
    public Task<MP3File> ParseIntoMP3(ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
    {
        var fileName = Encoding.Latin1.GetString(data.Slice(start: 24, length: 260).Span).Split('\0')[0].Normalize().Trim();
        var fileData = data[284..];
        return Task.FromResult(new MP3File() { Name = fileName, Data = fileData });
    }
}