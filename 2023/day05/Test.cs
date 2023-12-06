using Lib;

namespace day05;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 35)]
    [InlineData("input.txt", 836040384)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var groups = new List<Group>();
        for (var i = 2; i < input.Length; i++) {
            if (input[i] != "") {
                if (char.IsDigit(input[i][0])) {
                    var x = input[i].Split(' ').Select(long.Parse).ToArray();
                    groups.Last().Conversions.Add(new Conversion(x[1], x[1] + x[2] - 1, x[0] - x[1]));
                } else {
                    groups.Add(new Group(input[i].Substring(0, input[i].Length - 1)));
                }
            }
        }

        var seeds = input[0].Split(": ")[1].Split(' ').Select(long.Parse).ToArray();
        var results = new List<long>();
        foreach (var seed in seeds) {
            var result = seed;
            foreach (var group in groups) {
                result = group.Convert(result);
            }
            results.Add(result);
        }

        Assert.Equal(expectedResult, results.Min());
    }

    [Theory]
    [InlineData("sample.txt", 46)]
    [InlineData("input.txt", 10834440)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var groups = new List<Group>();
        for (var i = 2; i < input.Length; i++) {
            if (input[i] != "") {
                if (char.IsDigit(input[i][0])) {
                    var x = input[i].Split(' ').Select(long.Parse).ToArray();
                    groups.Last().Conversions.Add(new Conversion(x[1], x[1] + x[2] - 1, x[0] - x[1]));
                } else {
                    groups.Add(new Group(input[i].Substring(0, input[i].Length - 1)));
                }
            }
        }

        var seedRangesInput = input[0].Split(": ")[1].Split(' ').Select(long.Parse).ToArray();
        var seedRanges = new List<Range>();
        for (var i = 0; i < seedRangesInput.Length / 2; i++) {
            seedRanges.Add(new Range(seedRangesInput[i * 2], seedRangesInput[i * 2] + seedRangesInput[i * 2 + 1] - 1));
        }
        // var results = new List<long>();
        // foreach (var seed in seeds) {
        //     var result = seed;
        //     foreach (var group in groups) {
        //         result = group.Convert(result);
        //     }
        //     results.Add(result);
        // }
        groups.Reverse();

        long location = 0;
        for(location = 0; location < 836040384; location++) {
            var result = location;
            foreach (var group in groups) {
                result = group.ConvertBack(result);
            }
            if (seedRanges.Any(s => s.From <= result && s.To >= result))
                break;
        }

        Assert.Equal(expectedResult, location);
    }
}

public record Group(string Name) {
    public List<Conversion> Conversions {get;set;} = new List<Conversion>();

    public long ConvertBack(long output) {
        var c = Conversions.SingleOrDefault(c => c.From <= (output - c.Offset) && c.To >= (output - c.Offset));
        if (c == null)
            return output;
        return output - c.Offset;
    }

    public long Convert(long input) {
        var c = Conversions.SingleOrDefault(c => c.From <= input && c.To >= input);
        if (c == null)
            return input;
        return input + c.Offset;
    }
}

public record Conversion(long From, long To, long Offset);

public record Range(long From, long To);