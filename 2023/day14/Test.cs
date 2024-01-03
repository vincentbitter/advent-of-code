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
        var rocks = new List<Rock>();
        for(var row = 0; row < input.Length; row++) {
            var y = input.Length - row;
            for (var x = 0; x < input[0].Length; x++) {
                if (input[row][x] != '.') {
                    var rock = new Rock(input[row][x], x, y);
                    rocks.Add(rock);
                    rock.RollNorth(rocks, input.Length);
                }
            }
        }

        var roundRocks = rocks.Where(r => r.IsRound()).ToList();
        var total = roundRocks.Sum(r => r.Y);
        Assert.Equal(expectedResult, total);
    }

    [Theory]
    [InlineData("sample.txt", 64)]
    [InlineData("input.txt", 102055)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var rocks = new List<Rock>();
        for(var row = 0; row < input.Length; row++) {
            var y = input.Length - row;
            for (var x = 0; x < input[0].Length; x++) {
                if (input[row][x] != '.') {
                    var rock = new Rock(input[row][x], x, y);
                    rocks.Add(rock);
                }
            }
        }

        var maxY = input.Length;
        var maxX = input[0].Length - 1;
        List<double> history = new List<double>();
        var roundRocks = rocks.Where(r => r.IsRound()).ToList();
        var skipped = false;
        for (var spin = 1; spin <= 1_000_000_000; spin++) {
            // North
            for (var x = 0; x <= maxX; x++) {
                var orderedRocks = roundRocks.Where(r => r.X == x).OrderByDescending(r => r.Y).ToArray();
                foreach(var rock in orderedRocks)
                    rock.RollNorth(rocks, maxY);
            }

            // West
            for (var y = 0; y <= maxY; y++) {
                var orderedRocks = roundRocks.Where(r => r.Y == y).OrderBy(r => r.X).ToArray();
                foreach(var rock in orderedRocks)
                    rock.RollWest(rocks);
            }

            // South
            for (var x = 0; x <= maxX; x++) {
                var orderedRocks = roundRocks.Where(r => r.X == x).OrderBy(r => r.Y).ToArray();
                foreach(var rock in orderedRocks)
                    rock.RollSouth(rocks);
            }

            // East
            for (var y = 0; y <= maxY; y++) {
                var orderedRocks = roundRocks.Where(r => r.Y == y).OrderByDescending(r => r.X).ToArray();
                foreach(var rock in orderedRocks)
                    rock.RollEast(rocks, maxX);
            }
            
            if (!skipped) {
                var totalX = roundRocks.Sum(r => UniqueValue(r.X, r.Y));
                var total = roundRocks.Sum(r => r.Y);
                if (history.Any(r => r == totalX)) {
                    skipped = true;
                    var previous = history.IndexOf(totalX) + 1;
                    var size = previous - spin;
                    var left = (1_000_000_000 - spin) % size;
                    spin = 1_000_000_000 - left;
                }
                
                history.Add(totalX);
            }
        }

        var total2 = roundRocks.Sum(r => r.Y);
        Assert.Equal(expectedResult, total2);
    }

    private long UniqueValue(int x, int y) {
        return x > y 
            ? y | ((long)x << 32)
            : x | ((long)y << 32);
    }
}

public class Rock {
    private bool _round = false;
    public int X {get; private set;}
    public int Y {get; private set;}

    public Rock(char type, int x, int y) {
        X = x;
        Y = y;
        _round = type == 'O';
    }

    public bool IsRound() => _round;

    public void RollNorth(List<Rock> rocks, int maxY)
    {
        if (!IsRound())
            return;
        
        var rocksAbove = rocks.Where(r => r.Y > Y && r.X == X).ToArray();
        if (rocksAbove.Any())
            Y = rocksAbove.Min(r => r.Y) - 1;
        else
            Y = maxY;
    }

    public void RollWest(List<Rock> rocks)
    {
        if (!IsRound())
            return;
        
        var rocksAbove = rocks.Where(r => r.Y == Y && r.X < X).ToArray();
        if (rocksAbove.Any())
            X = rocksAbove.Max(r => r.X) + 1;
        else
            X = 0;
    }

    public void RollSouth(List<Rock> rocks)
    {
        if (!IsRound())
            return;
        
        var rocksAbove = rocks.Where(r => r.Y < Y && r.X == X).ToArray();
        if (rocksAbove.Any())
            Y = rocksAbove.Max(r => r.Y) + 1;
        else
            Y = 1;
    }

    public void RollEast(List<Rock> rocks, int maxX)
    {
        if (!IsRound())
            return;
        
        var rocksAbove = rocks.Where(r => r.Y == Y && r.X > X).ToArray();
        if (rocksAbove.Any())
            X = rocksAbove.Min(r => r.X) - 1;
        else
            X = maxX;
    }
}