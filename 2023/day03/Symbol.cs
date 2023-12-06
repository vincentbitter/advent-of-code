using Lib.Interfaces;

namespace day03;

public record Symbol (int X, int Y, char Value) : IMapItem {
    public int Width {get; }= 1;
    public int Height {get; } = 1;
}