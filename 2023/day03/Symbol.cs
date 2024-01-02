using Lib.Geometry;

namespace day03;

public record Symbol(int X, int Y, char Value) : Point2D(X, Y);