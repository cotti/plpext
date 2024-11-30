namespace Plpext.Core.Models;

public record MP3File
{
    public required string Name { get; init; }
    public ReadOnlyMemory<byte> Data { get; init; }
}
