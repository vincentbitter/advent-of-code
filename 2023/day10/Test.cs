using Lib;
using Lib.Geometry;
using Lib.Geometry.Extensions;

namespace day10;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 8, 'F')]
    [InlineData("input.txt", 6697, 'J')]
    public void PartA(string fileName, int expectedResult, char startingPositionChar)
    {
        var input = Parser.ReadAllLines(fileName);
        var map = new CharMap(input);
        var start = map.FindChar('S');
        map.SetChar(start, startingPositionChar);
        var pipes = GetPipeRoute(map, start);

        Assert.Equal(expectedResult, pipes.Count() / 2);
    }

    [Theory]
    [InlineData("sample.txt", 1, 'F')]
    [InlineData("sample2.txt", 10, 'F')]
    [InlineData("sample3.txt", 4, 'F')]
    [InlineData("input.txt", 423, 'J')]
    public void PartB(string fileName, int expectedResult, char startingPositionChar)
    {
        var input = Parser.ReadAllLines(fileName);
        var map = new CharMap(input);
        var start = map.FindChar('S');
        map.SetChar(start, startingPositionChar);
        var pipes = GetPipeRoute(map, start);

        map = ExplodeMap(map, pipes);
        map.AddBorder('.');

        var floodSize = map.FloodFillSize(new(0, 0), new[] { '.', ' ' }, new[] { '.' });
        var totalDots = map.Count('.');
        var count = totalDots - floodSize;

        Assert.Equal(expectedResult, count);
    }

    private static CharMap ExplodeMap(CharMap map, HashSet<Point2D> pipes)
    {
        var newInput = new string[map.Height * 2 - 1];
        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                var character = map.GetChar(x, y);

                if (pipes.Contains(new Point2D(x, y)))
                {
                    if (character == 'F' || character == 'L' || character == '-')
                        newInput[y * 2] += character + "-";
                    else if (character == 'J' || character == '7' || character == '|')
                        newInput[y * 2] += character + " ";

                    if (y < map.Height - 1)
                    {
                        if (character == 'F' || character == '7' || character == '|')
                            newInput[y * 2 + 1] += "| ";
                        else if (character == 'J' || character == 'L' || character == '-')
                            newInput[y * 2 + 1] += "  ";
                    }
                }
                else
                {
                    newInput[y * 2] += ". ";
                    if (y < map.Height - 1)
                        newInput[y * 2 + 1] += "  ";
                }
            }
        }
        return new CharMap(newInput);
    }

    private static HashSet<Point2D> GetPipeRoute(CharMap map, Point2D start)
    {
        var pipes = new HashSet<Point2D>();
        var previous = start;
        var position = previous;
        while (pipes.Add(position))
        {
            var newPosition = GetNextPosition(map.GetChar(position), previous, position);
            previous = position;
            position = newPosition;
        }
        return pipes;
    }

    private static Point2D GetNextPosition(char current, Point2D previous, Point2D position)
    {
        if (current == '|')
        {
            var toX = position.X;
            var toY = 2 * position.Y - previous.Y;
            return new(toX, toY);
        }
        if (current == '-')
        {
            var toX = 2 * position.X - previous.X;
            var toY = position.Y;
            return new(toX, toY);
        }
        if (current == 'L')
        {
            if (position.Y > previous.Y)
                return new(position.X + 1, position.Y); // Right
            return new(position.X, position.Y - 1); // Up
        }
        if (current == 'J')
        {
            if (position.Y > previous.Y)
                return new(position.X - 1, position.Y); // Left
            return new(position.X, position.Y - 1); // Up
        }
        if (current == '7')
        {
            if (position.Y < previous.Y)
                return new(position.X - 1, position.Y); // Left
            return new(position.X, position.Y + 1); // Down
        }
        if (current == 'F')
        {
            if (position.Y < previous.Y)
                return new(position.X + 1, position.Y); // Right
            return new(position.X, position.Y + 1); // Down
        }
        if (current == '.')
        {
            throw new NotImplementedException("Landed on the ground!");
        }
        throw new NotImplementedException("Don't recognize: " + current);
    }
}
