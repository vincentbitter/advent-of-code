
namespace day05;

public record struct Seed(long Value)
{
    public readonly Seed Convert(IEnumerable<ConversionGroup> groups)
    {
        return groups.Aggregate(this, (seed, group) => group.Convert(seed));
    }

    public readonly Seed Move(long offset)
    {
        return new Seed(Value + offset);
    }
}