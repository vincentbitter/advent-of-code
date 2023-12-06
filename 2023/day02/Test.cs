using Lib;

namespace day02;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 8)]
    [InlineData("input.txt", 1734)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var games = input.Select(line => new Game(line));
        var total = games.Where(g => g.IsPossible(12,14,13)).Sum(g => g.Number);
        Assert.Equal(expectedResult, total);
    }

    [Theory]
    [InlineData("sample.txt", 2286)]
    [InlineData("input.txt", 70387)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var games = input.Select(line => new Game(line));
        var total = games.Sum(g => g.PowerOfMinCubes());
        Assert.Equal(expectedResult, total);
    }
}