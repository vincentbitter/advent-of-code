using Lib.Extensions;
using Lib.Geometry;

namespace day03;

public record Number(int X, int Y, int Value) : Object2D(new Point2D(X, Y), new Point2D(X + Value.Length() - 1, Y));