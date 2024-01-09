using Lib;

namespace day21;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 6, 16)]
    [InlineData("input.txt", 64, 3617)]
    public void PartA(string fileName, int steps, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var maxX = input[0].Length - 1;
        var maxY = input.Length - 1;

        var startX = 0;
        var startY = 0;

        var rocks = new List<Location>();
        for (var y = 0; y <= maxY; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                var i = input[y][x];
                if (i == 'S')
                {
                    startX = x;
                    startY = y;
                }
                else if (i == '#')
                {
                    rocks.Add(new(x, y));
                }
            }
        }
        var queue = new HashSet<Tuple<Location, int>>
        {
            new(new(startX, startY), 0)
        };
        var destinations = new HashSet<Location>();
        var visitedUneven = new HashSet<Location>();
        while (queue.Count > 0)
        {
            var newQueue = new HashSet<Tuple<Location, int>>();
            foreach (var item in queue)
            {
                if (rocks.Contains(item.Item1))
                    continue;

                if (item.Item2 % 2 == 0)
                    destinations.Add(item.Item1);

                if (!visitedUneven.Add(item.Item1))
                    continue;

                if (item.Item2 == steps)
                    continue;

                var left = new Location(item.Item1.X - 1, item.Item1.Y);
                var right = new Location(item.Item1.X + 1, item.Item1.Y);
                var up = new Location(item.Item1.X, item.Item1.Y - 1);
                var down = new Location(item.Item1.X, item.Item1.Y + 1);

                if (item.Item1.X > 0 && !destinations.Contains(left))
                    newQueue.Add(new(left, item.Item2 + 1));
                if (item.Item1.X < maxX && !destinations.Contains(right))
                    newQueue.Add(new(right, item.Item2 + 1));
                if (item.Item1.Y > 0 && !destinations.Contains(up))
                    newQueue.Add(new(up, item.Item2 + 1));
                if (item.Item1.Y < maxY && !destinations.Contains(down))
                    newQueue.Add(new(down, item.Item2 + 1));
            }
            queue = newQueue;
        }

        Assert.Equal(expectedResult, destinations.Count);
    }

    [Theory]
    [InlineData("input.txt", 26501365, 596857397104703)]
    public void PartB(string fileName, int maxSteps, long expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var maxX = input[0].Length - 1;
        var maxY = input.Length - 1;

        var startX = 0;
        var startY = 0;

        var rocks = new List<Location>();
        for (var y = 0; y <= maxY; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                var i = input[y][x];
                if (i == 'S')
                {
                    startX = x;
                    startY = y;
                }
                else if (i == '#')
                {
                    rocks.Add(new(x, y));
                }
            }
        }

        long size = (maxSteps - startY) / (maxY + 1);
        var result = GetDestinations(new(startX, startY), maxX, maxY, rocks);

        var oddSquares = (size + 1) * (size + 1);
        var oddSquareSize = result.OddPositions.Count();
        var oddQuareCornerSize = result.OddPositions.Count(p => p.Item2 > 65);

        var evenSquares = size * size;
        var evenSquareSize = result.EvenPositions.Count();
        var evenSquareCornerSize = result.EvenPositions.Count(p => p.Item2 > 65);

        var total = oddSquares * oddSquareSize
            + evenSquares * evenSquareSize
            - (size + 1) * oddQuareCornerSize
            + size * evenSquareCornerSize;

        Assert.Equal(expectedResult, total);
    }

    private record DestinationReport(
        List<Tuple<Location, int>> EvenPositions,
        List<Tuple<Location, int>> OddPositions
    );

    private DestinationReport GetDestinations(
            Location startingPosition, int maxX, int maxY, List<Location> rocks)
    {
        var queue = new List<Tuple<Location, int>>
        {
            new(startingPosition, 0)
        };

        var visitedEven = new HashSet<Tuple<Location, int>>();
        var visitedUneven = new HashSet<Tuple<Location, int>>();
        var visitedEven2 = new HashSet<Location>();
        var visitedUneven2 = new HashSet<Location>();
        visitedEven2.Add(startingPosition);

        while (queue.Count > 0)
        {
            var filtered = queue.Where(item => !rocks.Any(r => r.X == item.Item1.X && r.Y == item.Item1.Y)).ToList();
            filtered = filtered.Where(item => item.Item2 % 2 == 0 ? visitedEven.Add(item) : visitedUneven.Add(item)).ToList();

            var newQueue = new List<Tuple<Location, int>>();
            newQueue.AddRange(filtered.Where(item => item.Item1.X > 0).Select(item => new Tuple<Location, int>(new(item.Item1.X - 1, item.Item1.Y), item.Item2 + 1)));
            newQueue.AddRange(filtered.Where(item => item.Item1.X < maxX).Select(item => new Tuple<Location, int>(new(item.Item1.X + 1, item.Item1.Y), item.Item2 + 1)));
            newQueue.AddRange(filtered.Where(item => item.Item1.Y > 0).Select(item => new Tuple<Location, int>(new(item.Item1.X, item.Item1.Y - 1), item.Item2 + 1)));
            newQueue.AddRange(filtered.Where(item => item.Item1.Y < maxY).Select(item => new Tuple<Location, int>(new(item.Item1.X, item.Item1.Y + 1), item.Item2 + 1)));
            queue = newQueue
                .Where(item => item.Item2 % 2 == 0
                    ? visitedEven2.Add(item.Item1)
                    : visitedUneven2.Add(item.Item1))
                .ToList();
        }

        return new(visitedEven.ToList(), visitedUneven.ToList());
    }
}

public record struct Location(int X, int Y);