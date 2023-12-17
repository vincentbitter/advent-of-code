using Lib;

namespace day17;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 1, 3, 102)]
    [InlineData("input.txt", 1, 3, 724)]
    [InlineData("sample.txt", 4, 10, 94)]
    [InlineData("input.txt", 4, 10, 877)]
    public void PartAB(string fileName, int min, int max, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var positions = new Position[input.Length][];
        for(var y = 0; y < input.Length; y++)
        {
            positions[y] = new Position[input[0].Length];
            for(var x = 0; x < input[0].Length; x++)
            {
                positions[y][x] = new Position(x, y, int.Parse("" + input[y][x]));
            }
        }
        foreach (var a in positions)
        {
            foreach(var p in a) {
                p.SetCanComeFrom(positions, min, max);
            }
        }

        var start = positions[0][0];
        var target = positions.Last().Last();

        var queue = new Queue<Position>();
        start.VerticalDirty = true;
        start.HorizontalDirty = true;
        queue.Enqueue(start);
        while (queue.Count > 0) {
            var item = queue.Dequeue();
            if (item.HorizontalDirty) {
                var destinations = item.CanComeFromHorizontal;
                foreach (var dest in destinations) {
                    var cost = item.CostVertical + dest.GetCost(item);
                    if (cost < dest.CostHorizontal || dest.CostHorizontal == 0) {
                        dest.CostHorizontal = cost;
                        dest.VerticalDirty = true;
                        if (!queue.Contains(dest))
                            queue.Enqueue(dest);
                    }
                }
                item.HorizontalDirty = false;
            }
            if (item.VerticalDirty) {
                var destinations = item.CanComeFromVertical;
                foreach (var dest in destinations) {
                    var cost = item.CostHorizontal + dest.GetCost(item);
                    if (cost < dest.CostVertical || dest.CostVertical == 0) {
                        dest.CostVertical = cost;
                        dest.HorizontalDirty = true;
                        if (!queue.Contains(dest))
                            queue.Enqueue(dest);
                    }
                }
                item.VerticalDirty = false;
            }
        }
        var diff = target.Value - start.Value;
        var result = Math.Min(target.CostHorizontal, target.CostVertical) + diff;
        Assert.Equal(expectedResult, result);
    }
}

public class Position {
    public int X {get;set;}
    public int Y {get;set;}
    public int Value {get;set;}

    public Position(int x, int y, int value)
    {
        X = x;
        Y = y;
        Value = value;
    }

    public List<Position> CanComeFromHorizontal { get; private set; } = new List<Position>();
    public List<Position> CanComeFromVertical { get; private set; } = new List<Position>();

    public bool HorizontalDirty {get;set;}
    public bool VerticalDirty {get;set;}

    public int CostHorizontal { get; internal set; }
    public int CostVertical { get; internal set; }

    public void SetCanComeFrom(Position[][] positions, int min, int max) {
        var total = 0;
        for (var x = X + 1; x <= Math.Min(positions[0].Length - 1, X + max); x++) {
            var pos = positions[Y][x];
            if (pos == null)
                break;
            total += pos.Value;

            if (x >= X + min) {
                CostCache.Add(pos, total);
                CanComeFromHorizontal.Add(pos);

                pos.CostCache.Add(this, total - pos.Value + Value);
                pos.CanComeFromHorizontal.Add(this);
            }
        }
        total = 0;
        for (var y = Y + 1; y <= Math.Min(positions.Length - 1, Y + max); y++) {
            var pos = positions[y][X];
            if (pos == null)
                break;
            total += pos.Value;

            if (y >= Y + min) {
                CostCache.Add(pos, total);
                CanComeFromVertical.Add(pos);

                pos.CostCache.Add(this, total - pos.Value + Value);
                pos.CanComeFromVertical.Add(this);
            }
        }
    }

    public Dictionary<Position, int> CostCache {get; private set;} = new Dictionary<Position, int>();
    public int GetCost(Position position)
    {
        return CostCache[position];
    }
}

public enum Direction {
    Horizontal,
    Vertical
}