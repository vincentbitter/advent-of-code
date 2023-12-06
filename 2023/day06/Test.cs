using Lib;

namespace day06;

public class Test
{
    public long GetMinPressTime(double distance, double maxTime)
    {
        return (long) Math.Floor((maxTime - Math.Sqrt(maxTime * maxTime - 4d * distance)) / 2d) + 1;
    }

    public long CountBetterPressTimes(long maxTime, long minDistance)
    {
        var minPressTime = GetMinPressTime(minDistance, maxTime);
        var maxPressTime = maxTime - minPressTime;
        return maxPressTime - minPressTime + 1;
    }

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
}