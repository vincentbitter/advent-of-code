using Lib.Interfaces;

namespace day03;

public record Number (int X, int Y, int Width, int Value) : IMapItem {
    public int Height { get; } = 1;
}