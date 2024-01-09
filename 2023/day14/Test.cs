using Lib;

namespace day14;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 136)]
    [InlineData("input.txt", 111979)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var total = 0;

        for (var x = 0; x < input[0].Length; x++)
        {
            var maxY = input.Length;
            for (var row = 0; row < input.Length; row++)
            {
                var y = input.Length - row;

                if (input[row][x] != '.')
                {
                    var rock = new Rock(input[row][x], x, y);
                    if (rock.IsRound())
                    {
                        rock.Y = maxY;
                        total += rock.Y;
                    }
                    maxY = rock.Y - 1;
                }
            }
        }

        Assert.Equal(expectedResult, total);
    }

    [Theory]
    [InlineData("sample.txt", 64)]
    [InlineData("input.txt", 102055)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var rocks = new List<Rock>();
        for (var row = 0; row < input.Length; row++)
        {
            var y = input.Length - row;
            for (var x = 0; x < input[0].Length; x++)
            {
                if (input[row][x] != '.')
                {
                    var rock = new Rock(input[row][x], x, y);
                    rocks.Add(rock);
                }
            }
        }

        var maxY = input.Length;
        var maxX = input[0].Length - 1;
        var history = new HashSet<long>();
        var roundRocks = rocks.Where(r => r.IsRound()).ToList();
        var skipped = false;
        var orderedRocks = rocks
            .GroupBy(r => r.X)
            .OrderBy(r => r.Key)
            .Select(g => g.OrderByDescending(r => r.Y).ToList())
            .ToArray();

        for (var spin = 1; spin <= 1_000_000_000; spin++)
        {
            // North
            var orderedRocks2 = Enumerable.Range(0, maxY + 1).Select(i => new List<Rock>()).ToArray();
            for (var x = 0; x <= maxX; x++)
            {
                var max = maxY;
                foreach (var rock in orderedRocks[x])
                {
                    max = rock.RollToY(max) - 1;
                    orderedRocks2[rock.Y].Add(rock);
                }
            }

            orderedRocks = orderedRocks2;
            orderedRocks2 = Enumerable.Range(0, maxX + 1).Select(i => new List<Rock>()).ToArray();
            // West
            for (var y = 0; y <= maxY; y++)
            {
                var min = 0;
                foreach (var rock in orderedRocks[y])
                {
                    min = rock.RollToX(min) + 1;
                    orderedRocks2[rock.X].Add(rock);
                }
            }

            orderedRocks = orderedRocks2;
            orderedRocks2 = Enumerable.Range(0, maxY + 1).Select(i => new List<Rock>()).ToArray();
            // South
            for (var x = maxX; x >= 0; x--)
            {
                var min = 1;
                foreach (var rock in orderedRocks[x])
                {
                    min = rock.RollToY(min) + 1;
                    orderedRocks2[rock.Y].Add(rock);
                }
            }

            orderedRocks = orderedRocks2;
            orderedRocks2 = Enumerable.Range(0, maxX + 1).Select(i => new List<Rock>()).ToArray();
            // East
            for (var y = maxY; y >= 0; y--)
            {
                var max = maxX;
                foreach (var rock in orderedRocks[y])
                {
                    max = rock.RollToX(max) - 1;
                    orderedRocks2[rock.X].Add(rock);
                }
            }
            orderedRocks = orderedRocks2;

            if (!skipped)
            {
                var totalX = roundRocks.Sum(r => (long)r.GetHashCode());
                var total = roundRocks.Sum(r => r.Y);
                if (!history.Add(totalX))
                {
                    skipped = true;
                    var previous = history.ToList().IndexOf(totalX) + 1;
                    var size = previous - spin;
                    var left = (1_000_000_000 - spin) % size;
                    spin = 1_000_000_000 - left;
                }
            }
        }

        var total2 = roundRocks.Sum(r => r.Y);
        Assert.Equal(expectedResult, total2);
    }
}

public record Rock
{
    private bool _round = false;
    public int X { get; set; }
    public int Y { get; set; }

    public Rock(char type, int x, int y)
    {
        X = x;
        Y = y;
        _round = type == 'O';
    }

    public bool IsRound() => _round;

    public int RollToY(int y)
    {
        if (IsRound())
            Y = y;
        return Y;
    }

    public int RollToX(int x)
    {
        if (IsRound())
            X = x;
        return X;
    }
}