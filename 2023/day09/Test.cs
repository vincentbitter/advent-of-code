using Lib;

namespace day09;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 114)]
    [InlineData("input.txt", 1731106378)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var nextValues = input
            .Select(line => line.Split(' '))
            .Select(v => v.Select(int.Parse).ToArray())
            .Select(NextValue)
            .ToArray();
        Assert.Equal(expectedResult, nextValues.Sum());
    }

    [Theory]
    [InlineData("sample.txt", 2)]
    [InlineData("input.txt", 1087)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var nextValues = input
            .Select(line => line.Split(' '))
            .Select(v => v.Select(int.Parse).ToArray())
            .Select(PreviousValue)
            .ToArray();
        Assert.Equal(expectedResult, nextValues.Sum());
    }

    private int NextValue(int[] values)
    {
        if (values.Distinct().Count() == 1)
            return values[0];

        var diffs = new List<int>();
        for (var i = 1; i < values.Length; i++)
            diffs.Add(values[i] - values[i - 1]);
        var increment = NextValue(diffs.ToArray());
        return values.Last() + increment;
    }

    private int PreviousValue(int[] values)
    {
        if (values.Distinct().Count() == 1)
            return values[0];

        var diffs = new List<int>();
        for (var i = 1; i < values.Length; i++)
            diffs.Add(values[i] - values[i - 1]);
        var increment = PreviousValue(diffs.ToArray());
        return values.First() - increment;
    }
}