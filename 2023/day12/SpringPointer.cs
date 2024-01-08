namespace day12;

public record struct SpringPointer(int Spring, int Index)
{
    public readonly SpringPointer Move(Spring[] springs, int value)
    {
        if (springs[Spring].Size > Index + value)
            return new SpringPointer(Spring, Index + value);

        return new SpringPointer(Spring + 1, 0);
    }
}
