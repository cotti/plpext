using MP3Sharp;
using Plpext.Core.Interfaces;
using Plpext.Core.Models;

namespace Plpext.Core.AudioConverter
{
    public class MP3AudioConverter : IAudioConverter
    {
        private readonly IMP3Parser _parser;

        public MP3AudioConverter(IMP3Parser parser)
        {
            _parser = parser;
        }
        public async Task<AudioFile> ConvertAudioAsync(ReadOnlyMemory<byte> file, CancellationToken cancellationToken)
        {
            var baseFile = await _parser.ParseIntoMP3(file, cancellationToken);
            byte[] pcmData = null!;
            using var mp3Stream = new MP3Stream(new MemoryStream(baseFile.Data.ToArray()));
            using var pcmStream = new MemoryStream();
            var buffer = new byte[4096];
            int bytesReturned = 1;
            int totalBytesRead = 0;
            
            try
            {
                while (bytesReturned > 0)
                {
                    try
                    {
                        bytesReturned = await mp3Stream.ReadAsync(buffer, 0, buffer.Length);
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        Console.WriteLine($"File reached a corrupted/non-compliant portion. Total bytes read: {totalBytesRead} | File: {baseFile.Name}");
                        break;
                    }
                    catch (NullReferenceException ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        Console.WriteLine($"File reached a corrupted/non-compliant portion. Total bytes read: {totalBytesRead} | File: {baseFile.Name}");
                        break;
                    }
                    totalBytesRead += bytesReturned;
                    await pcmStream.WriteAsync(buffer, 0, bytesReturned);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception details:");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                    Console.WriteLine($"InnerException StackTrace: {ex.InnerException.StackTrace}");
                }
            }
            pcmData = ResampleToMono(pcmStream.ToArray());
            
            var audioDuration = (double)(pcmData.Length / 2) / mp3Stream.Frequency;

            return new AudioFile() { Name = baseFile.Name, Data = pcmData, Duration = TimeSpan.FromSeconds(audioDuration), Format = AudioFormat.Mono16, Frequency = mp3Stream.Frequency };
        }

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
}
