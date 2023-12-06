namespace day02;

public record Game {
    public int Number { get; init; }
    public IEnumerable<Turn> Turns { get; init; }

    public Game(string line) {
        var items = line.Split(": ");
        Number = int.Parse(items[0][5..]);
        Turns = items[1].Split("; ").Select(t => new Turn(t)).ToArray();
    }

    public bool IsPossible(int maxRed, int maxBlue, int maxGreen) {
        return Turns.All(t => t.IsPossible(maxRed, maxBlue, maxGreen));
    }

    public int PowerOfMinCubes()
    {
        var minRed = Turns.Select(t => t.Red).Max();
        var minBlue = Turns.Select(t => t.Blue).Max();
        var minGreen = Turns.Select(t => t.Green).Max();
        return minRed * minBlue * minGreen;
    }
}