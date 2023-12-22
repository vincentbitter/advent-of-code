using Lib;

namespace day22;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 5)]
    [InlineData("input.txt", 473)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var bricks = input.Select(l => l.Split('~').Select(p => p.Split(',').Select(int.Parse).ToArray()).ToArray())
            .Select(x => new Brick(
                new Point3D(x[0][0], x[0][1], x[0][2]), 
                new Point3D(x[1][0], x[1][1], x[1][2])
            ))
            .ToList();

        var allBricks = bricks.OrderBy(b => b.Bottom).ToList();
        foreach (var brick in allBricks) {
            brick.Lower(allBricks);
        }

        var removed = new List<Brick>();
        foreach (var brick in bricks) {
            var supporting = brick.GetBrickDirectlyAbove(bricks.ToList());
            if (!supporting.Any(b => b.GetBrickDirectlyBelow(bricks).Count == 1))
                removed.Add(brick);
        }
        
        Assert.Equal(expectedResult, removed.Count);
    }

    [Theory]
    [InlineData("sample.txt", 7)]
    [InlineData("input.txt", 61045)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var bricks = input.Select(l => l.Split('~').Select(p => p.Split(',').Select(int.Parse).ToArray()).ToArray())
            .Select(x => new Brick(
                new Point3D(x[0][0], x[0][1], x[0][2]), 
                new Point3D(x[1][0], x[1][1], x[1][2])
            ))
            .ToList();

        var allBricks = bricks.OrderBy(b => b.Bottom).ToList();
        foreach (var brick in allBricks) {
            brick.Lower(allBricks);
        }

        foreach (var brick in bricks.OrderBy(b => b.Bottom)) {
            var supporting = brick.GetBrickDirectlyBelow(bricks.ToList());
            brick.SupportedBy.AddRange(supporting);
        };

        foreach (var brick in bricks) {
            brick.Supporting = brick.GetBrickDirectlyAbove(bricks.ToList());
            brick.SingleSupporter = brick.Supporting.Any(b => b.GetBrickDirectlyBelow(bricks).Count == 1);
        }

        var total = 0;
        var i = 0;
        foreach(var brick in bricks.Where(b => b.SingleSupporter)) {
            i++;
            total += brick.GetDamage(bricks);
        }
        
        Assert.Equal(expectedResult, total);
    }
}

public record Brick
{
    public Point3D From {get;set;}
    public Point3D To {get;set;}

    public bool SingleSupporter {get;set;}
    public List<Brick> SupportedBy {get;set;} = new List<Brick>();

    public Brick(Point3D from, Point3D to) {
        From = from;
        To = to;
    }

    public int Bottom => From.Z;
    public int Top => To.Z;

    public List<Brick> Supporting { get; set; } = new List<Brick>();

    public int GetDamage(List<Brick> bricks) {
        bricks = bricks.Where(b => b.Bottom > Top).ToList();
        var removed = new HashSet<Brick>();
        removed.Add(this);
        var queue = new HashSet<Brick>();
        Supporting.ForEach(b => queue.Add(b));
        while (queue.Count > 0) {
            var newQueue = new HashSet<Brick>();
            foreach (var brick in queue) {
                if (!brick.SupportedBy.Except(removed).Any()) {
                    brick.Supporting.Except(removed).ToList().ForEach(b => newQueue.Add(b));
                    removed.Add(brick);
                }
            }
            queue = newQueue;
        }
        return removed.Count - 1;
    }

    public List<Brick> GetBrickDirectlyAbove(List<Brick> otherBricks) {
        return otherBricks
            .Where(b => b.Bottom - 1 == Top)
            .Where(b => OverlapsX(b) && OverlapsY(b))
            .ToList();
    }

    public List<Brick> GetBrickDirectlyBelow(List<Brick> otherBricks) {
        return otherBricks
            .Where(b => b.Top + 1 == Bottom)
            .Where(b => OverlapsX(b) && OverlapsY(b))
            .ToList();
    }

    public IEnumerable<Brick> GetBrickBelow(List<Brick> bricks) {
        return bricks
            .Where(b => b.Bottom < Bottom)
            .Where(b => OverlapsX(b) && OverlapsY(b))
            .ToList();
    }

    private bool OverlapsX(Brick b)
    {
        if (From.X >= b.From.X && From.X <= b.To.X)
            return true;
        if (To.X >= b.From.X && To.X <= b.To.X)
            return true;
        if (From.X < b.From.X && To.X > b.To.X)
            return true;
        return false;
    }

    private bool OverlapsY(Brick b)
    {
        if (From.Y >= b.From.Y && From.Y <= b.To.Y)
            return true;
        if (To.Y >= b.From.Y && To.Y <= b.To.Y)
            return true;
        if (From.Y < b.From.Y && To.Y > b.To.Y)
            return true;
        return false;
    }

    public void Lower(List<Brick> otherBricks)
    {
        var below = GetBrickBelow(otherBricks);

        var height = To.Z - From.Z;
        if (below.Any())
            From = From with { Z = below.Max(b => b.Top) + 1 };
        else
            From = From with { Z = 1 };
        To = To with { Z = From.Z + height };
    }
}

public record Point3D (int X, int Y, int Z);