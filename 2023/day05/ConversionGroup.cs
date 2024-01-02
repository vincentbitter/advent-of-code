using Lib.Misc;

namespace day05;

public record ConversionGroup(string Name)
{
    public List<Conversion> Conversions { get; set; } = new List<Conversion>();

    public IEnumerable<SeedRange> Convert(SeedRange seedRange)
    {
        var result = new List<SeedRange>();
        foreach (var conversion in Conversions)
        {
            if (conversion.Range.Overlaps(seedRange.Range, out var overlap))
            {
                result.Add(new SeedRange(overlap.Move(conversion.Offset)));
            }
        }

        result.AddRange(seedRange.Range.Except(Conversions.Select(c => c.Range)).Select(r => new SeedRange(r)));

        return result;
    }

    public Seed Convert(Seed seed)
    {
        var c = Conversions.SingleOrDefault(c => c.Range.From <= seed.Value && c.Range.To >= seed.Value);
        if (c != null)
            return seed.Move(c.Offset);

        return seed;
    }
}