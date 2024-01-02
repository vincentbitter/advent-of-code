using Lib.Misc;

namespace day05;

public record Input(string[] Lines)
{
    public IEnumerable<Seed> ParseSeeds()
    {
        return Lines[0]
            .Split(": ")[1]
            .Split(' ')
            .Select(long.Parse)
            .Select(v => new Seed(v))
            .ToArray();
    }

    public List<SeedRange> ParseSeedRanges()
    {
        var seedRangesInput = Lines[0].Split(": ")[1].Split(' ').Select(long.Parse).ToArray();
        var seedRanges = new List<LongRange>();
        for (var i = 0; i < seedRangesInput.Length / 2; i++)
        {
            seedRanges.Add(new LongRange(seedRangesInput[i * 2], seedRangesInput[i * 2] + seedRangesInput[i * 2 + 1] - 1));
        }

        return seedRanges.Select(r => new SeedRange(r)).ToList();
    }

    public List<ConversionGroup> ParseGroups()
    {
        var groups = new List<ConversionGroup>();
        for (var i = 2; i < Lines.Length; i++)
        {
            if (Lines[i] != "")
            {
                if (char.IsDigit(Lines[i][0]))
                {
                    var x = Lines[i].Split(' ').Select(long.Parse).ToArray();
                    groups.Last().Conversions.Add(new Conversion(new LongRange(x[1], x[1] + x[2] - 1), x[0] - x[1]));
                }
                else
                {
                    groups.Add(new ConversionGroup(Lines[i].Substring(0, Lines[i].Length - 1)));
                }
            }
        }

        return groups;
    }
}