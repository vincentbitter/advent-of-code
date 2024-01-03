namespace Lib.Geometry.Extensions;

public static class CharMapFloodFill
{
    public static int FloodFillSize(this CharMap map, Point2D start,
        IEnumerable<char> include, IEnumerable<char>? count = null)
    {
        count = count ?? include;

        var visited = new HashSet<Point2D>();
        var next = new List<Point2D> { start };
        var floodSize = 0;
        do
        {
            var newNext = new List<Point2D>();
            foreach (var p in next)
            {
                if (!visited.Add(p))
                    continue;

                var c = map.GetChar(p);
                if (include.Contains(c))
                {
                    if (count.Contains(c))
                        floodSize++;
                    newNext.AddRange(map.GetNeighbors(p));
                }
            }
            next = newNext;
        } while (next.Any());

        return floodSize;
    }
}