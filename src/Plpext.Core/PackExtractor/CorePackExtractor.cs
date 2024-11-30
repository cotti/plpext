using Plpext.Core.Interfaces;
using System.Diagnostics;

namespace Plpext.Core.PackExtractor
{
    public class CorePackExtractor : IPackExtractor
    {
        private static readonly byte[] filePattern = { 0x53, 0x4E, 0x44, 0x55, 0x00 };
        public async Task<IEnumerable<ReadOnlyMemory<byte>>> GetFileListAsync(string filePath, CancellationToken cancellationToken)
        {
            ReadOnlyMemory<byte> file = await File.ReadAllBytesAsync(filePath, cancellationToken);
            var fileIndexes = FindFileIndexes(file.Span);

            var result = new List<ReadOnlyMemory<byte>>();
            for (int i = 0; i < fileIndexes.Count - 1; ++i)
            {
                var nextFile = file.Slice(start: fileIndexes[i], length: fileIndexes[i + 1] - fileIndexes[i]);
                result.Add(nextFile);
            }
            return result;
        }

        private static List<int> FindFileIndexes(ReadOnlySpan<byte> file)
        {
            var result = new List<int>();


            int[] skipTable = new int[256];
            for (int i = 0; i < skipTable.Length; i++) skipTable[i] = filePattern.Length;
            for (int i = 0; i < filePattern.Length - 1; i++) skipTable[filePattern[i]] = filePattern.Length - i - 1;

            int idx = 0;
            while (idx <= file.Length - filePattern.Length)
            {
                int j = filePattern.Length - 1;
                while (j >= 0 && filePattern[j] == file[idx + j]) j--;

                if (j < 0)
                {
                    result.Add(idx);
                    idx += filePattern.Length;
                }
                else
                {
                    idx += skipTable[file[idx + j]];
                }
            }
            result.Add(file.Length - 1);
            return result;
        }
    }
}
