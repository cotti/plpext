using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plpext.Core.Interfaces;

public interface IPackExtractor
{
    Task<IEnumerable<ReadOnlyMemory<byte>>> GetFileListAsync(string filePath, CancellationToken cancellationToken);
}
