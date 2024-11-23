using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plpext.Core.Models
{
    public record AudioFile
    {
        public required string Name { get; init; }
        public ReadOnlyMemory<byte> Data { get; init; }
        public TimeSpan Duration { get; init; }
        public AudioFormat Format { get; init; }
        public int Frequency { get; init; }
    }
}
