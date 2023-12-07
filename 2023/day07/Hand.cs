namespace day07;

public class Hand : IComparable
{
    private char[] Cards {get;}
    public int Bid {get;}
    public int Score {get;}
    private bool _jokerRule;

    public Hand(string cards, int bid, bool jokerRule) {
        _jokerRule = jokerRule;
        Cards = cards.ToArray();
        Bid = bid;
        Score = CalculateScore(cards);
    }

    private int CalculateScore(IEnumerable<char> cards)
    {
        var groups = cards.GroupBy(c => c).ToArray();
        var orderedGroupsBase = _jokerRule
            ? groups.Where(g => g.Key != 'J')
            : groups;
        var orderedGroups = orderedGroupsBase.OrderByDescending(g => g.Count()).ToArray();

        var jokers = groups.SingleOrDefault(g => g.Key == 'J')?.Count() ?? 0;

        var bestSet = _jokerRule
            ? jokers == 5 ? 5 : orderedGroups[0].Count() + jokers
            : orderedGroups[0].Count();

        if (bestSet == 5)
            return 7;

        var secondBestSet = orderedGroups[1].Count();

        if (bestSet == 1)
            return 1;

        // 2 Pair
        if (bestSet == 2 && secondBestSet == 2)
            return 3;

        if (bestSet == 2)
            return 2;

        // Full House
        if (bestSet == 3 && secondBestSet == 2)
            return 5;

        if (bestSet == 3)
            return 4;
        
        if (bestSet == 4)
            return 6;

        throw new Exception("What!?");
    }

    private static List<char> _cardValues = new List<char> {
        '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'
    };

    private static List<char> _cardValuesWithJokerRule = new List<char> {
        'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A'
    };

    private int CompareCard(char a, char b) {
        var cardValues = _jokerRule ? _cardValuesWithJokerRule : _cardValues;
        var aScore = cardValues.IndexOf(a);
        var bScore = cardValues.IndexOf(b);
        if (aScore > bScore)
            return 1;
        if (bScore > aScore)
            return -1;
        return 0;
    }

    public int CompareTo(object? obj)
    {
        var other = obj as Hand;
        if (other == null)
            throw new Exception("Can only compare hands!");

        if (Score > other.Score)
            return 1;
        if (other.Score > Score)
            return -1;
        
        for (var i = 0; i < 5; i++) {
            var result = CompareCard(Cards[i], other.Cards[i]);
            if (result != 0)
                return result;
        }
        return 0;
    }
}