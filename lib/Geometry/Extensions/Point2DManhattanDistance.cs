namespace Lib.Geometry.Extensions;

public static class Point2DManhattanDistance
{
    public static int ManhattanDistance(this Point2D left, Point2D right)
    {
        return Math.Abs(left.X - right.X) + Math.Abs(left.Y - right.Y);
    }
}