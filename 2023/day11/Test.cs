using Lib;
using Lib.Geometry;
using Lib.Geometry.Extensions;
namespace day11;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 374)]
    [InlineData("input.txt", 9769724)]
    public void PartA(string fileName, int expectedResult)
    {
        PartB(fileName, 2, expectedResult);
    }

    [Theory]
    [InlineData("sample.txt", 10, 1030)]
    [InlineData("sample.txt", 100, 8410)]
    [InlineData("input.txt", 1_000_000, 603020563700)]
    public void PartB(string fileName, int multiplier, long expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);

        // Add 9 empty rows instead of 10, because there is already 1 line
        multiplier--;

        // Find empty columns
        var xEmpty = new List<int>();
        for (var x = 0; x < input[0].Length; x++)
        {
            if (!input.Any(l => l[x] == '#'))
                xEmpty.Add(x);
        }

        // Find galaxy locations
        var galaxies = new List<Point2D>();
        var yOffset = 0;
        for (var y = 0; y < input.Length; y++)
        {
            var anyGalaxyOnRow = false;
            for (var x = 0; x < input[0].Length; x++)
            {
                if (input[y][x] == '#')
                {
                    galaxies.Add(new Point2D(
                        x + xEmpty.Count(i => i < x) * multiplier,
                        y + yOffset * multiplier
                    ));
                    anyGalaxyOnRow = true;
                }
            }

            // Increase row offset on empty row
            if (!anyGalaxyOnRow)
                yOffset++;
        }

        // Find shortest routes
        var result = 0L;
        for (var i = 0; i < galaxies.Count - 1; i++)
        {
            var from = galaxies[i];
            for (var j = i + 1; j < galaxies.Count; j++)
            {
                result += from.ManhattanDistance(galaxies[j]);
            }
        }

        Assert.Equal(expectedResult, result);

    }
}