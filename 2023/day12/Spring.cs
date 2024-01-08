namespace day12;

public struct Spring
{
    public bool RequiresSpring { get; init; }
    public int Size { get; init; }

    private string _value;

    public Spring(string input)
    {
        RequiresSpring = input.Contains('#');
        _value = input;
        Size = input.Length;
    }

    public readonly int FirstSpringAfter(int index)
    {
        for (var i = index; i < Size; i++)
        {
            if (_value[i] == '#')
                return i;
        }
        return -1;
    }

    public readonly bool RequiresSpringAt(int index)
    {
        return _value[index] == '#';
    }
}