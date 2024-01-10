namespace day22;

public class Stack
{
    private IEnumerable<Brick> _bricks;

    public Stack(string[] stack)
    {
        _bricks = stack
            .Select(l => l
                .Split('~')
                .Select(p => p.Split(',').Select(int.Parse).ToArray())
                .ToArray()
            )
            .Select(x => new Brick(
                new(x[0][0], x[0][1], x[0][2]),
                new(x[1][0], x[1][1], x[1][2])
            ))
            .OrderBy(b => b.Bottom)
            .ToList();
    }

    public void LowerBricks()
    {
        _bricks = _bricks
            .Aggregate(new List<Brick>(), (list, brick) =>
            {
                list.Add(LowerBrick(brick, list));
                return list;
            })
            .ToList();
    }

    private static Brick LowerBrick(Brick brick, IEnumerable<Brick> bricksBelow)
    {
        var bottom = FindNextBlock(brick, bricksBelow);

        var from = brick.From with { Z = bottom + 1 };
        var to = brick.To with { Z = from.Z + brick.Height };
        return brick with { From = from, To = to };
    }

    private static int FindNextBlock(Brick brick, IEnumerable<Brick> bricksBelow)
    {
        var top = 0;
        foreach (var other in bricksBelow)
        {
            if (other.OverlapsXY(brick) && other.Top > top)
                top = other.Top;
        }
        return top;
    }

    public int CanSafelyRemove()
    {
        var bricksByTop = _bricks.ToLookup(b => b.Top);
        var bricksByBottom = _bricks.ToLookup(b => b.Bottom);
        return _bricks.Count(brick =>
        {
            var supporting = GetOverlapping(brick, bricksByBottom[brick.Top + 1]);
            return !supporting.Any(b => GetOverlapping(b, bricksByTop[b.Bottom - 1]).Count == 1);
        });
    }

    public int MaxDamage()
    {
        return GetSingleSupporters()
            .Sum(b => b.GetDamage());
    }

    private IEnumerable<Brick> GetSingleSupporters()
    {
        var bricksByTop = _bricks.ToLookup(b => b.Top);
        var bricksByBottom = _bricks.ToLookup(b => b.Bottom);
        foreach (var brick in _bricks)
        {
            brick.SupportedBy = GetOverlapping(brick, bricksByTop[brick.Bottom - 1]);
            brick.Supporting = GetOverlapping(brick, bricksByBottom[brick.Top + 1]);
        }

        return _bricks.Where(brick => brick.IsSingleSupporter()).ToArray();
    }

    private static List<Brick> GetOverlapping(Brick brick, IEnumerable<Brick> otherBricks)
    {
        return otherBricks
            .Where(brick.OverlapsXY)
            .ToList();
    }
}