using Lib.Misc;

namespace day05;

public record struct SeedRange(LongRange Range)
{
    public readonly IEnumerable<SeedRange> Convert(IEnumerable<ConversionGroup> groups)
    {
        IEnumerable<SeedRange> ranges = new[] { this };
        return groups
            .Aggregate(ranges, (ranges, group) =>
                ranges.SelectMany(r => group.Convert(r)));
    }

    public readonly long LowestOutput(IEnumerable<ConversionGroup> groups)
    {
        var converted = Convert(groups);
        return converted.Min(r => r.Range.From);
    }
}