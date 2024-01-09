namespace Lib.Misc;

public record struct LongRange(long From, long To)
{
    public readonly bool Overlaps(LongRange other, out LongRange overlap)
    {
        overlap = new LongRange(Math.Max(From, other.From), Math.Min(To, other.To));
        return overlap.From <= overlap.To;
    }

    public readonly IEnumerable<LongRange> Except(IEnumerable<LongRange> others)
    {
        var rest = new List<LongRange> { this };
        foreach (var other in others)
        {
            rest = rest.SelectMany(r => r.Except(other)).ToList();
        }
        return rest;
    }

    public readonly IEnumerable<LongRange> Except(LongRange other)
    {
        if (From < other.From)
            yield return new LongRange(From, Math.Min(other.From - 1, To));
        if (To > other.To)
            yield return new LongRange(Math.Max(other.To + 1, From), To);
    }

    public readonly LongRange Move(long offset)
    {
        return new LongRange(From + offset, To + offset);
    }

    public readonly IEnumerable<LongRange> Split(int value)
    {
        return new List<LongRange> {
            new (From, value - 1),
            new (value, To)
        };
    }

    public readonly long Size()
    {
        return To - From + 1;
    }

    public readonly double Avg()
    {
        return (To + From) / 2.0;
    }
}