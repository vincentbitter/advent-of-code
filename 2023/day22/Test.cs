using Lib;

namespace day22;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 5)]
    [InlineData("input.txt", 473)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var stack = new Stack(input);
        stack.LowerBricks();

        var result = stack.CanSafelyRemove();

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("sample.txt", 7)]
    [InlineData("input.txt", 61045)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var stack = new Stack(input);
        stack.LowerBricks();

        var result = stack.MaxDamage();

        Assert.Equal(expectedResult, result);
    }
}