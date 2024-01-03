


namespace Lib.Geometry;

public class CharMap
{
    private string[] _map;

    public int Width => _map[0].Length;
    public int Height => _map.Length;

    public CharMap(string[] map)
    {
        SetMap(map);
    }

    public char GetChar(Point2D point)
    {
        return GetChar(point.X, point.Y);
    }

    public char GetChar(int x, int y)
    {
        return _map[y][x];
    }

    public Point2D FindChar(char c)
    {
        var line = _map.Single(line => line.Contains(c));
        var x = line.IndexOf(c);
        var y = _map.ToList().IndexOf(line);
        return new(x, y);
    }

    public IEnumerable<Point2D> GetNeighbors(Point2D point)
    {
        if (point.X < Width - 1)
            yield return new(point.X + 1, point.Y);
        if (point.Y < Height - 1)
            yield return new(point.X, point.Y + 1);
        if (point.X > 0)
            yield return new(point.X - 1, point.Y);
        if (point.Y > 0)
            yield return new(point.X, point.Y - 1);
    }

    private void SetMap(string[] map)
    {
        _map = map;
    }

    public void AddBorder(char c)
    {
        var map = new string[_map.Length + 2];
        map[0] = "".PadLeft(_map[0].Length + 2, c);
        map[map.Length - 1] = map[0];
        for (var y = 0; y < _map.Length; y++)
            map[y + 1] = c + _map[y] + c;

        SetMap(map);
    }

    public int Count(char value)
    {
        return _map.Sum(l => l.Count(c => c == value));
    }

    public void SetChar(Point2D point, char value)
    {
        SetChar(point.X, point.Y, value);
    }

    public void SetChar(int x, int y, char value)
    {
        _map[y] = _map[y].Substring(0, x) + value + _map[y].Substring(x + 1);
    }
}