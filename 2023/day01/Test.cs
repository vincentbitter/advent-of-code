using Lib;

namespace day01;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 142)]
    [InlineData("input.txt", 55477)]
    public void PartA(string fileName, int expectedResult)
    {
        var calibrationDocument = Parser.ReadAllLines(fileName);
        var total = calibrationDocument
            .Select(line => new CalibrationValue(line))
            .Sum(value => value.GetValue());

        Assert.Equal(expectedResult, total);
    }

    [Theory]
    [InlineData("sample2.txt", 281)]
    [InlineData("sample3.txt", 28)]
    [InlineData("input.txt", 54431)]
    public void PartB(string fileName, int expectedResult)
    {
        var calibrationDocument = Parser.ReadAllLines(fileName);
        var total = calibrationDocument
            .Select(line => new CalibrationValue(line))
            .Sum(value => value.GetValue(true));

        Assert.Equal(expectedResult, total);
    }
}