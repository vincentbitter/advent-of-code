namespace day07;

public class Game
{
    private readonly IEnumerable<Hand> _hands;

    public Game(IEnumerable<string> input, bool jokerRule) {
        _hands = input
            .Select(line => new Hand(
                line.Split(' ')[0],
                int.Parse(line.Split(' ')[1]),
                jokerRule
            ))
            .ToArray();
    }

    public long GetScore() {
        var hands = _hands.OrderBy(h => h).ToArray();
        long total = 0;
        for(var i = 0; i < hands.Length; i++) {
            total += hands[i].Bid * (i + 1);
        }
        return total;
    }
}
