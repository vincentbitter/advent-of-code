namespace Lib.Geometry;

public class Map2D<T> where T : Object2D
{
    private IEnumerable<T> _objects;

    public Map2D()
    {
        _objects = new HashSet<T>();
    }

    public Map2D(IEnumerable<T> objects)
    {
        _objects = objects;
    }

    public IEnumerable<T> FindAdjacent(Point2D point)
    {
        return _objects.Where(item =>
                item.From.Y <= point.Y + 1
                && item.To.Y >= point.Y - 1
                && item.From.X <= point.X + 1
                && item.To.X >= point.X - 1
            ).ToList();
    }
}