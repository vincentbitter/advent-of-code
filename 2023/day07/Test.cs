using Lib;

namespace day07;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 6440)]
    [InlineData("input.txt", 251545216)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var game = new Game(input, false);

        Assert.Equal(expectedResult, game.GetScore());
    }

    [Theory]
    [InlineData("sample.txt", 5905)]
    [InlineData("input.txt", 250384185)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var game = new Game(input, true);

        Assert.Equal(expectedResult, game.GetScore());
    }
}