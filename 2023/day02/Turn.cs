namespace day02;

public record Turn {
    public int Red { get; init; }
    public int Blue {get; init; }
    public int Green {get; init; }

    public Turn(string details) {
        var items = details.Split(", ");
        foreach (var item in items) {
            if (item.EndsWith(" red"))
                Red = int.Parse(item[..^3]);
            if (item.EndsWith(" blue"))
                Blue = int.Parse(item[..^4]);
            if (item.EndsWith(" green"))
                Green = int.Parse(item[..^5]);
        }
    }

    public bool IsPossible(int maxRed, int maxBlue, int maxGreen)
    {
       return Red <= maxRed && Blue <= maxBlue && Green <= maxGreen;
    }
}