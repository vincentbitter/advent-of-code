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

        var rocks = new List<Tuple<int, int>>();
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
        var queue = new Queue<Tuple<int, int, int>>();
        queue.Enqueue(new(startX, startY, 0));
        var destinations = new List<Tuple<int, int>>();
        var visitedUneven = new List<Tuple<int, int>>();
        while (queue.Count > 0)
        {
            var item = queue.Dequeue();

            if (visitedUneven.Any(r => r.Item1 == item.Item1 && r.Item2 == item.Item2))
                continue;

            if (rocks.Any(r => r.Item1 == item.Item1 && r.Item2 == item.Item2))
                continue;

            if (item.Item3 % 2 == 0)
                destinations.Add(new(item.Item1, item.Item2));
            else
                visitedUneven.Add(new(item.Item1, item.Item2));
            if (item.Item3 == steps)
                continue;

            if (item.Item1 > 0 && !destinations.Any(d => d.Item1 == item.Item1 - 1 && d.Item2 == item.Item2))
                queue.Enqueue(new(item.Item1 - 1, item.Item2, item.Item3 + 1));
            if (item.Item1 < maxX && !destinations.Any(d => d.Item1 == item.Item1 + 1 && d.Item2 == item.Item2))
                queue.Enqueue(new(item.Item1 + 1, item.Item2, item.Item3 + 1));
            if (item.Item2 > 0 && !destinations.Any(d => d.Item1 == item.Item1 && d.Item2 == item.Item2 - 1))
                queue.Enqueue(new(item.Item1, item.Item2 - 1, item.Item3 + 1));
            if (item.Item2 < maxY && !destinations.Any(d => d.Item1 == item.Item1 && d.Item2 == item.Item2 + 1))
                queue.Enqueue(new(item.Item1, item.Item2 + 1, item.Item3 + 1));
        }

        var uniqueDestinations = destinations.Distinct().ToArray();
        Assert.Equal(expectedResult, uniqueDestinations.Length);
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

        var rocks = new List<Tuple<int, int>>();
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
        var oddQuareCornerSize = result.OddPositions.Count(p => p.Item3 > 65);

        var evenSquares = size * size;
        var evenSquareSize = result.EvenPositions.Count();
        var evenSquareCornerSize = result.EvenPositions.Count(p => p.Item3 > 65);

        var total = oddSquares * oddSquareSize
            + evenSquares * evenSquareSize
            - (size + 1) * oddQuareCornerSize
            + size * evenSquareCornerSize;

        Assert.Equal(expectedResult, total);
    }

    private record DestinationReport(
        List<Tuple<int, int, int>> EvenPositions,
        List<Tuple<int, int, int>> OddPositions
    );

    private DestinationReport GetDestinations(
            Tuple<int, int> startingPosition, int maxX, int maxY, List<Tuple<int, int>> rocks)
    {
        var queue = new List<Tuple<int, int, int>>();
        queue.Add(new(startingPosition.Item1, startingPosition.Item2, 0));

        var visitedEven = new HashSet<Tuple<int, int, int>>();
        var visitedUneven = new HashSet<Tuple<int, int, int>>();
        var visitedEven2 = new HashSet<string>();
        var visitedUneven2 = new HashSet<string>();
        visitedEven2.Add(startingPosition.Item1 + " " + startingPosition.Item2);

        while (queue.Count > 0)
        {
            var filtered = queue.Where(item => !rocks.Any(r => r.Item1 == item.Item1 && r.Item2 == item.Item2)).ToList();
            filtered = filtered.Where(item => item.Item3 % 2 == 0 ? visitedEven.Add(item) : visitedUneven.Add(item)).ToList();

            var newQueue = new List<Tuple<int, int, int>>();
            newQueue.AddRange(filtered.Where(item => item.Item1 > 0).Select(item => new Tuple<int, int, int>(item.Item1 - 1, item.Item2, item.Item3 + 1)));
            newQueue.AddRange(filtered.Where(item => item.Item1 < maxX).Select(item => new Tuple<int, int, int>(item.Item1 + 1, item.Item2, item.Item3 + 1)));
            newQueue.AddRange(filtered.Where(item => item.Item2 > 0).Select(item => new Tuple<int, int, int>(item.Item1, item.Item2 - 1, item.Item3 + 1)));
            newQueue.AddRange(filtered.Where(item => item.Item2 < maxY).Select(item => new Tuple<int, int, int>(item.Item1, item.Item2 + 1, item.Item3 + 1)));
            queue = newQueue
                .Where(item => item.Item3 % 2 == 0
                    ? visitedEven2.Add(item.Item1 + " " + item.Item2)
                    : visitedUneven2.Add(item.Item1 + " " + item.Item2))
                .ToList();
        }

        return new(visitedEven.ToList(), visitedUneven.ToList());
    }
}