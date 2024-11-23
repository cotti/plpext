using Plpext.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plpext.Core.Interfaces
{
    public interface IMP3Parser
    {
        Task<MP3File> ParseIntoMP3(ReadOnlyMemory<byte> data, CancellationToken cancellationToken);
    }
}
