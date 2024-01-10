namespace Lib.Geometry;

public record Object3D(Point3D From, Point3D To)
{
    public int Top => To.Z;
    public int Bottom => From.Z;
    public int Height => To.Z - From.Z;

    public bool OverlapsXY(Object3D other)
    {
        return OverlapsX(other) && OverlapsY(other);
    }

    public bool OverlapsX(Object3D b)
    {
        if (From.X >= b.From.X && From.X <= b.To.X)
            return true;
        if (To.X >= b.From.X && To.X <= b.To.X)
            return true;
        if (From.X < b.From.X && To.X > b.To.X)
            return true;
        return false;
    }

    public bool OverlapsY(Object3D b)
    {
        if (From.Y >= b.From.Y && From.Y <= b.To.Y)
            return true;
        if (To.Y >= b.From.Y && To.Y <= b.To.Y)
            return true;
        if (From.Y < b.From.Y && To.Y > b.To.Y)
            return true;
        return false;
    }
}