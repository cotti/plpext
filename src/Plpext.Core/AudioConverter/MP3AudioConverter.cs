using MP3Sharp;
using Plpext.Core.Interfaces;
using Plpext.Core.Models;

namespace Plpext.Core.AudioConverter;

public class MP3AudioConverter : IAudioConverter
{
    private readonly IMP3Parser _parser;

    public MP3AudioConverter(IMP3Parser parser)
    {
        _parser = parser;
    }
    public async Task<AudioFile> ConvertAudioAsync(ReadOnlyMemory<byte> mp3Input, CancellationToken cancellationToken)
    {
        var baseFile = await _parser.ParseIntoMP3(mp3Input, cancellationToken);
        byte[] pcmData = null!;
        await using var mp3Stream = new MP3Stream(new MemoryStream(baseFile.Data.ToArray()));
        await using var pcmStream = new MemoryStream();
        var buffer = new byte[4096];
        int bytesReturned = 1;
        int totalBytesRead = 0;

        try
        {
            while (bytesReturned > 0)
            {
                try
                {
                    bytesReturned = await mp3Stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                }
                catch (IndexOutOfRangeException)
                {
                    //File reached a corrupted/non-compliant portion.
                    break;
                }
                catch (NullReferenceException)
                {
                    //File reached a corrupted/non-compliant portion that caused MP3Sharp to throw a NRE.
                    break;
                }
                totalBytesRead += bytesReturned;
                await pcmStream.WriteAsync(buffer, 0, bytesReturned, cancellationToken);
            }

            pcmData = ResampleToMono(pcmStream.ToArray());

            var audioDuration = (double)(pcmData.Length / 2) / mp3Stream.Frequency;

            return new AudioFile() { Name = baseFile.Name, MP3Data = mp3Input, Data = pcmData, Duration = TimeSpan.FromSeconds(audioDuration), Format = AudioFormat.Mono16, Frequency = mp3Stream.Frequency };
        }
        catch (Exception)
        {
            //An exception here would be something new.
        }
        return new AudioFile() { Name = $"${baseFile.Name} failed extraction", Data = ReadOnlyMemory<byte>.Empty, MP3Data = ReadOnlyMemory<byte>.Empty, Duration = TimeSpan.FromSeconds(0), Format = AudioFormat.Unknown, Frequency = 0 };
    }

    /* This is pretty cursed, but it works.
       I don't remember where I got it from the first time, though. I think LLMs weren't a thing yet.
    */
    private static byte[] ResampleToMono(Span<byte> data)
    {
        byte[] newData = new byte[data.Length / 2];

        for (int i = 0; i < data.Length / 4; ++i)
        {
            int HI = 1;
            int LO = 0;
            short left = (short)((data[i * 4 + HI] << 8) | (data[i * 4 + LO] & 0xff));
            short right = (short)((data[i * 4 + 2 + HI] << 8) | (data[i * 4 + 2 + LO] & 0xff));
            int avg = (left + right) / 2;

            newData[i * 2 + HI] = (byte)((avg >> 8) & 0xff);
            newData[i * 2 + LO] = (byte)((avg & 0xff));
        }

        return newData;
    }
}
