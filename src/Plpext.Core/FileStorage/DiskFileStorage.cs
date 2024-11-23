using Plpext.Core.Interfaces;
using Plpext.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plpext.Core.FileStorage
{
    public class DiskFileStorage : IFileStorage
    {
        public async Task SaveFilesAsync(IEnumerable<MP3File> files, string targetPath)
        {
            HashSet<string> names = new HashSet<string>();
            try
            {
                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return;
            }

            foreach (var file in files)
            {
                var finalFileName = TryAddName(names, string.Join("_", file.Name.Split(Path.GetInvalidFileNameChars())));
                try
                {
                    await File.Create(Path.Combine(targetPath, $"{finalFileName}.mp3")).WriteAsync(file.Data);
                }
                catch (Exception e) { Debug.WriteLine(e); }
            }
        }
        private static string TryAddName(HashSet<string> names, string newName)
        {
            if (!names.Add(newName))
                return TryAddName(names, newName + "_");
            return newName;
        }
    }
}
