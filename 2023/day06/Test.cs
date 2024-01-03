using Lib;
using Lib.Algebra;

namespace day06;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 288)]
    [InlineData("input.txt", 114400)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var maxTimes = input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var minDistances = input[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        long total = 1;
        for (var i = 0; i < maxTimes.Length; i++)
        {
            total *= CountBetterPressTimes(maxTimes[i], minDistances[i]);
        }
        Assert.Equal(expectedResult, total);
    }

    [Theory]
    [InlineData("sample.txt", 71503)]
    [InlineData("input.txt", 21039729)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var maxTime = long.Parse(input[0].Split(':')[1].Replace(" ", ""));
        var minDistance = long.Parse(input[1].Split(':')[1].Replace(" ", ""));

        var total = CountBetterPressTimes(maxTime, minDistance);
        Assert.Equal(expectedResult, total);
    }

    // (pressTime * (maxTime - pressTime)) = distance
    // (pressTime)(maxTime - pressTime) - distance = 0
    // -(pressTime^2) + (maxTime * pressTime) - distance = 0
    // Quadratic equation: ax^2 + bx + c
    // x = pressTime
    // a = -1
    // b = maxTime
    // c = -distance
    public long CountBetterPressTimes(long maxTime, long minDistance)
    {
        var equation = new QuadraticEquation(-1, maxTime, -minDistance);
        var result = equation.Solve();
        var minPressTime = (long) Math.Floor(result.Item2) + 1;
        var maxPressTime = (long) Math.Ceiling(result.Item1) - 1;
        return maxPressTime - minPressTime + 1;
    }
}