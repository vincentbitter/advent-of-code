using Lib;

namespace day04;

public partial class Test
{
    [Theory]
    [InlineData("sample.txt", 13)]
    [InlineData("input.txt", 23441)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var games = input.Select(Game.Create).ToArray();
        var total = 0;
        foreach (var game in games)
        {
            var wins = game.CountWins();
            var points = (int)Math.Pow(2, wins - 1);
            total += points;
        }

        Assert.Equal(expectedResult, total);
    }

    [Theory]
    [InlineData("sample.txt", 30)]
    [InlineData("input.txt", 5923918)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var games = input.Select(Game.Create).ToArray();

        for (var line = 0; line < games.Length; line++)
        {
            var good = games[line].CountWins();
            for (var i = 1; i <= good; i++)
                games[line + i].Copies += games[line].Copies;
        }

        Assert.Equal(expectedResult, games.Sum(g => g.Copies));
    }
}