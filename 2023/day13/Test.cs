using Lib;
using Lib.Extensions;

namespace day13;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 405)]
    [InlineData("sample2.txt", 2)]
    [InlineData("input.txt", 39939)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName).SplitByValue("");

        var verticalCount = 0;
        var horizontalCount = 0;
        foreach (var map in input)
        {
            var rotatedMap = map.RotateClockwise();
            horizontalCount += MirroredRowsScore(map);
            verticalCount += MirroredRowsScore(rotatedMap);
        }

        Assert.Equal(expectedResult, (horizontalCount * 100) + verticalCount);
    }

    [Theory]
    [InlineData("sample.txt", 400)]
    [InlineData("input.txt", 32069)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName).SplitByValue("");

        var verticalCount = 0;
        var horizontalCount = 0;
        foreach (var map in input)
        {
            var rotatedMap = map.RotateClockwise();
            var h = MirroredRowsScore(map);
            var v = MirroredRowsScore(rotatedMap);

            horizontalCount += MirroredRowScoreWithRepair(map, h - 1);
            verticalCount += MirroredRowScoreWithRepair(rotatedMap, v - 1);
        }

        Assert.Equal(expectedResult, (horizontalCount * 100) + verticalCount);
    }

    private int MirroredRowsScore(string[] map, int skipIndex = -1)
    {
        var centers = FindCenterIndexes(map);
        foreach (var center in centers)
        {
            if (center == skipIndex)
                continue;

            var i = center;
            var j = center + 1;
            while (i >= 0 && j < map.Length && map[i] == map[j])
            {
                i--;
                j++;
            }

            var max = (center + 1) * 2;
            if (center >= map.Length / 2)
                max = (map.Length - center - 1) * 2;
            var rows = j - i - 1;
            if (max == rows)
                return center + 1;
        }
        return 0;
    }

    private static IEnumerable<int> FindCenterIndexes(string[] map)
    {
        for (var i = 1; i < map.Length; i++)
        {
            if (map[i] == map[i - 1])
            {
                yield return i - 1;
            }
        }
    }

    private int MirroredRowScoreWithRepair(string[] map, int skipIndex)
    {
        var top = 0;
        for (var row = 0; row < map.Length; row++)
        {
            for (var column = 0; column < map[0].Length; column++)
            {
                var newMap = map.ToArray();
                newMap[row] = newMap[row].Substring(0, column)
                    + (map[row][column] == '.' ? '#' : '.')
                    + newMap[row].Substring(column + 1);
                var score = MirroredRowsScore(newMap, skipIndex);
                if (score > top)
                    top = score;
            }
        }
        return top;
    }
}