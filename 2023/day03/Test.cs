using Lib;

namespace day03;

public partial class Test
{
    [Theory]
    [InlineData("sample.txt", 4361)]
    [InlineData("input.txt", 546563)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var numberMap = new NumberMap(input);
        var symbols = numberMap.GetAllSymbols();
        var output = numberMap.GetAllNumbers()
            .Where(n => AdjacentFinder<Symbol>.FindAdjacentItems(symbols, n).Any())
            .Sum(n => n.Value);
        Assert.Equal(expectedResult, output);
    }

    [Theory]
    [InlineData("sample.txt", 467835)]
    [InlineData("input.txt", 91031374)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var numberMap = new NumberMap(input);
        var numbers = numberMap.GetAllNumbers();
        var symbols = numberMap.GetAllSymbols().Where(s => s.Value == '*');

        var output = 0;
        foreach (var symbol in symbols) {
            var adjacent = AdjacentFinder<Number>.FindAdjacentItems(numbers, symbol).ToArray();
            if (adjacent.Length == 2) {
                output += adjacent[0].Value * adjacent[1].Value;
            }
        }

        Assert.Equal(expectedResult, output);
    }
}