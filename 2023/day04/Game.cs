namespace day04;

public partial class Test
{
    public record Game(int Number, IEnumerable<int> WinningNumbers, IEnumerable<int> MyNumbers)
    {
        public int Copies { get; set; } = 1;

        public int CountWins()
        {
            return MyNumbers.Intersect(WinningNumbers).Count();
        }

        public static Game Create(string line)
        {
            var parts = line.Split(": ");
            var game = int.Parse(parts[0].Substring(5));
            var sets = parts[1].Split(" | ");
            var winningNumbers = sets[0].Split(' ').Where(s => s != "").Select(int.Parse).ToArray();
            var myNumbers = sets[1].Split(' ').Where(s => s != "").Select(int.Parse).ToArray();

            return new Game(game, winningNumbers, myNumbers);
        }
    }
}