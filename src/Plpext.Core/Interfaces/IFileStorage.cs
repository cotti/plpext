using Plpext.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plpext.Core.Interfaces
{
    public interface IFileStorage
    {
        Task SaveFilesAsync(IEnumerable<MP3File> files, string targetPath);
    }
}
