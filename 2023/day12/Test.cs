using Lib;

namespace day12;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 21)]
    [InlineData("input.txt", 7204)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var result = input.Select(i => i.Split(' ')).Select(i => CountPossibleArrangements(i[0], i[1].Split(',').Select(int.Parse).ToArray())).ToArray();
        Assert.Equal(expectedResult, result.Sum());
    }

    [Theory]
    [InlineData("sample.txt", 525152)]
    [InlineData("input.txt", 1672318386674)]
    public void PartB(string fileName, long expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var result = input.Select(i => i.Split(' ')).Select(i => {
            Console.WriteLine(i);
            return CountPossibleArrangements(
                ExtendRange(i[0], 5, '?'),
                ExtendRange(i[1], 5, ',').Split(',').Select(int.Parse).ToArray()
            );
        }).ToArray();
        Assert.Equal(expectedResult, result.Sum());
    }

    private string ExtendRange(string original, int times, char separator) {
        var a = new string[times];
        Array.Fill(a, original);
        return string.Join(separator, a);
    }

    private long CountPossibleArrangements(string springs, int[] groupSizes)
    {
        return CountPossibleArrangementsWithCache(
            springs.Split('.', StringSplitOptions.RemoveEmptyEntries), 
            groupSizes, 
            new Dictionary<string, long>()
        );
    }

    private long CountPossibleArrangementsWithCache(string[] springs, int[] groupSizes, Dictionary<string,long> cache)
    {
        var cacheKey = springs.Sum(s => s.Length) + "-" + groupSizes.Length;
        if (!cache.ContainsKey(cacheKey))
            cache[cacheKey] = CountPossibleArrangements(springs, groupSizes, cache);
        return cache[cacheKey];
    }

    private long CountPossibleArrangements(string[] springs, int[] groupSizes, Dictionary<string,long> cache)
    {
        if (springs.Length == 0 && groupSizes.Length == 0)
            return 1;

        if (springs.Length == 0 && groupSizes.Length > 0)
            return 0;

        var total = 0L;
        var leftOverSprings = springs.Skip(1).ToArray();
        var firstSpring = springs.Length > 0 ? springs[0].IndexOf('#') : -1;

        // If spring required, but next spring is too big or there is no next spring, 
        // then return 0, because no combination is possible
        if (firstSpring >= 0 && (groupSizes.Length == 0 || springs[0].Length < groupSizes[0]))
            return 0;

        // Skip group if no required springs,
        // and try first group at any position
        if (firstSpring == -1) {
            total += CountPossibleArrangementsWithCache(leftOverSprings, groupSizes, cache);

            if (groupSizes.Length == 0 || groupSizes[0] > springs[0].Length)
                return total;

            var maxIndex = springs[0].Length - groupSizes[0];
            
            if (groupSizes.Length == 1 && leftOverSprings.Any(l => l.Contains('#')))
                return total;

            // If no more groups left, return amount of options
            if (groupSizes.Length == 1)
                return total + maxIndex + 1;

            var leftGroupSizes = groupSizes.Skip(1).ToArray();
            for (var i = 0; i <= maxIndex; i++) {
                var a = new List<string>();
                if (springs[0].Length > i + groupSizes[0] + 1)
                    a.Add(springs[0].Substring(i + groupSizes[0] + 1));
                a.AddRange(leftOverSprings);
                total += CountPossibleArrangementsWithCache(a.ToArray(), leftGroupSizes, cache);
            }
        } 
        // Try fit group before first required spring,
        // and on first required spring
        else {
            // Before required spring
            var maxIndex = firstSpring - 1 - groupSizes[0];

            var leftGroupSizes = groupSizes.Skip(1).ToArray();
            for (var i = 0; i <= maxIndex; i++) {
                var a = new List<string>();
                if (springs[0].Length > i + groupSizes[0] + 1)
                    a.Add(springs[0].Substring(i + groupSizes[0] + 1));
                a.AddRange(leftOverSprings);
                total += CountPossibleArrangementsWithCache(a.ToArray(), leftGroupSizes, cache);
            }

            // On required spring
            maxIndex = Math.Min(firstSpring, springs[0].Length - groupSizes[0]);
            var minIndex = Math.Max(0, firstSpring + 1 - groupSizes[0]);
            for (var i = minIndex; i <= maxIndex; i++) {
                // Ignore positions if next to required spring
                if (springs[0].Length > i + groupSizes[0] && springs[0][i + groupSizes[0]] == '#')
                    continue;

                var a = new List<string>();
                if (springs[0].Length > i + groupSizes[0] + 1)
                    a.Add(springs[0].Substring(i + groupSizes[0] + 1));
                a.AddRange(leftOverSprings);
                total += CountPossibleArrangementsWithCache(a.ToArray(), leftGroupSizes, cache);
            }
        }

        return total;
    }
}