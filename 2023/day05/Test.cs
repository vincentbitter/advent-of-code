using Lib;

namespace day05;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 35)]
    [InlineData("input.txt", 836040384)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = new Input(Parser.ReadAllLines(fileName));
        var groups = input.ParseGroups();
        var seeds = input.ParseSeeds();

        var min = seeds.Min(seed => seed.Convert(groups).Value);

        Assert.Equal(expectedResult, min);
    }

    [Theory]
    [InlineData("sample.txt", 46)]
    [InlineData("input.txt", 10834440)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = new Input(Parser.ReadAllLines(fileName));
        var groups = input.ParseGroups();
        var seedRanges = input.ParseSeedRanges();

        var min = seedRanges.Min(seedRange => seedRange.LowestOutput(groups));

        Assert.Equal(expectedResult, min);
    }
}