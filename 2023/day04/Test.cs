using Lib;

namespace day04;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 13)]
    [InlineData("input.txt", 23441)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var total = 0;
        foreach (var line in input) {
            var parts = line.Split(": ");
            var sets = parts[1].Split(" | ");;
            var game = int.Parse(parts[0].Substring(5));
            var winningNumbers = sets[0].Split(' ').Where(s => s != "").Select(int.Parse).ToArray();
            var myNumbers = sets[1].Split(' ').Where(s => s != "").Select(int.Parse).ToArray();

            var points = 0;
            foreach (var number in myNumbers) {
                if (winningNumbers.Contains(number))
                    points = points == 0 ? 1 : points * 2;
            }
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
        var copies = Enumerable.Repeat(1, input.Length).ToArray();
        for(var line = 0; line < input.Length; line++) {
            var parts = input[line].Split(": ");
            var sets = parts[1].Split(" | ");
            var winningNumbers = sets[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var myNumbers = sets[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            var good = myNumbers.Count(winningNumbers.Contains);
            for (var i = 1; i <= good; i++)
                if (copies.Length > line + i)
                    copies[line + i] += copies[line];
        }

        Assert.Equal(expectedResult, copies.Sum());
    }
}