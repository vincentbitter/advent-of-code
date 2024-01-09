using Lib;

namespace day16;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 46)]
    [InlineData("input.txt", 7307)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var total = CountTiles(input, 0, 0, Direction.Right);
        Assert.Equal(expectedResult, total);
    }

    [Theory]
    [InlineData("sample.txt", 51)]
    [InlineData("input.txt", 7635)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);

        var results = new List<int>();
        for (var x = 0; x < input[0].Length; x++)
        {
            results.Add(CountTiles(input, x, 0, Direction.Down));
            results.Add(CountTiles(input, x, input.Length - 1, Direction.Up));
        }
        for (var y = 0; y < input.Length; y++)
        {
            results.Add(CountTiles(input, 0, y, Direction.Right));
            results.Add(CountTiles(input, input[0].Length - 1, y, Direction.Left));
        }

        Assert.Equal(expectedResult, results.Max());
    }

    private static int CountTiles(string[] map, int x, int y, Direction direction)
    {
        var beams = new List<Beam> { new(x, y, direction) };
        var visited = new HashSet<Location>();
        var visited2 = new HashSet<Move>();
        while (beams.Any())
        {
            foreach (var beam in beams)
                visited.Add(new Location(beam.X, beam.Y));

            beams = beams.Where(b => visited2.Add(new Move(b.X, b.Y, b.Direction)))
                .SelectMany(b => b.MoveNext(map)).ToList();
        }
        return visited.Count();
    }
}

public record struct Location(int X, int Y);
public record struct Move(int X, int Y, Direction Direction);

public record Beam
{
    public int X { get; set; }
    public int Y { get; set; }
    public Direction Direction { get; set; }

    public Beam(int x, int y, Direction direction)
    {
        X = x;
        Y = y;
        Direction = direction;
    }

    public IEnumerable<Beam> MoveNext(string[] map)
    {
        var type = GetPositionType(map);
        if (Direction == Direction.Right)
        {
            if ((type == '.' || type == '-') && X < map[0].Length - 1)
            {
                X++;
                Direction = Direction.Right;
                yield return this;
            }
            else if (type == '/' && Y > 0)
            {
                Y--;
                Direction = Direction.Up;
                yield return this;
            }
            else if (type == '\\' && Y < map.Length - 1)
            {
                Y++;
                Direction = Direction.Down;
                yield return this;
            }
            else if (type == '|')
            {
                if (Y > 0)
                {
                    yield return new Beam(X, Y - 1, Direction.Up);
                }
                if (Y < map.Length - 1)
                {
                    Direction = Direction.Down;
                    Y++;
                    yield return this;
                }
            }
        }
        else if (Direction == Direction.Left)
        {
            if ((type == '.' || type == '-') && X > 0)
            {
                X--;
                Direction = Direction.Left;
                yield return this;
            }
            else if (type == '/' && Y < map.Length - 1)
            {
                Y++;
                Direction = Direction.Down;
                yield return this;
            }
            else if (type == '\\' && Y > 0)
            {
                Y--;
                Direction = Direction.Up;
                yield return this;
            }
            else if (type == '|')
            {
                if (Y > 0)
                {
                    yield return new Beam(X, Y - 1, Direction.Up);
                }
                if (Y < map.Length - 1)
                {
                    Y++;
                    Direction = Direction.Down;
                    yield return this;
                }
            }
        }
        else if (Direction == Direction.Up)
        {
            if ((type == '.' || type == '|') && Y > 0)
            {
                Y--;
                Direction = Direction.Up;
                yield return this;
            }
            else if (type == '/' && X < map[0].Length - 1)
            {
                X++;
                Direction = Direction.Right;
                yield return this;
            }
            else if (type == '\\' && X > 0)
            {
                X--;
                Direction = Direction.Left;
                yield return this;
            }
            else if (type == '-')
            {
                if (X > 0)
                {
                    yield return new Beam(X - 1, Y, Direction.Left);
                }
                if (X < map[0].Length - 1)
                {
                    X++;
                    Direction = Direction.Right;
                    yield return this;
                }
            }
        }
        else if (Direction == Direction.Down)
        {
            if ((type == '.' || type == '|') && Y < map.Length - 1)
            {
                Y++;
                Direction = Direction.Down;
                yield return this;
            }
            else if (type == '/' && X > 0)
            {
                X--;
                Direction = Direction.Left;
                yield return this;
            }
            else if (type == '\\' && X < map[0].Length - 1)
            {
                X++;
                Direction = Direction.Right;
                yield return this;
            }
            else if (type == '-')
            {
                if (X > 0)
                {
                    yield return new Beam(X - 1, Y, Direction.Left);
                }
                if (X < map[0].Length - 1)
                {
                    X++;
                    Direction = Direction.Right;
                    yield return this;
                }
            }
        }
    }

    private char GetPositionType(string[] map)
    {
        return map[Y][X];
    }
}

public enum Direction
{
    Left,
    Right,
    Up,
    Down
}