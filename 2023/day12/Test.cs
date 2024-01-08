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
        var result = input
            .Select(i => i.Split(' '))
            .Select(i => CountPossibleArrangements(
                i[0],
                i[1].Split(',').Select(int.Parse).ToArray()
            ))
            .ToArray();
        Assert.Equal(expectedResult, result.Sum());
    }

    [Theory]
    [InlineData("sample.txt", 525152)]
    [InlineData("input.txt", 1672318386674)]
    public void PartB(string fileName, long expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var result = input
            .Select(i => i.Split(' '))
            .Select(i => CountPossibleArrangements(
                ExtendRange(i[0], 5, '?'),
                ExtendRange(i[1], 5, ',').Split(',').Select(int.Parse).ToArray()
            )).ToArray();
        Assert.Equal(expectedResult, result.Sum());
    }

    private string ExtendRange(string original, int times, char separator)
    {
        var a = new string[times];
        Array.Fill(a, original);
        return string.Join(separator, a);
    }

    private long CountPossibleArrangements(string springs, int[] groupSizes)
    {
        var springsArray = springs
            .Split('.', StringSplitOptions.RemoveEmptyEntries)
            .Select(i => new Spring(i))
            .ToArray();

        return CountPossibleArrangementsWithCache(
            springsArray,
            new SpringPointer(0, 0),
            groupSizes,
            0,
            new Dictionary<int, long>()
        );
    }

    private long CountPossibleArrangementsWithCache(Spring[] springs, SpringPointer pointer, int[] groupSizes, int groupSkip, Dictionary<int, long> cache)
    {
        var cacheKey = (pointer.Spring * 10000) + (pointer.Index * 100) + groupSkip;

        if (!cache.TryGetValue(cacheKey, out var result))
        {
            result = CountPossibleArrangements(springs, pointer, groupSizes, groupSkip, cache);
            cache[cacheKey] = result;
        }
        return result;
    }

    private long CountPossibleArrangements(Spring[] springs, SpringPointer pointer, int[] groupSizes, int groupSkip, Dictionary<int, long> cache)
    {
        if (springs.Length == pointer.Spring)
            return groupSizes.Length > groupSkip ? 0 : 1;

        var total = 0L;
        var firstSpring = springs[pointer.Spring].FirstSpringAfter(pointer.Index);
        var groupSize = groupSizes.Length > groupSkip ? groupSizes[groupSkip] : 0;

        // If spring required, but next spring is too big or there is no next spring, 
        // then return 0, because no combination is possible
        if (firstSpring >= 0 && (groupSizes.Length == groupSkip || springs[pointer.Spring].Size - pointer.Index < groupSize))
            return 0;

        // Skip group if no required springs,
        // and try first group at any position
        if (firstSpring == -1)
        {
            // Try skip group
            total += CountPossibleArrangementsWithCache(springs, new SpringPointer(pointer.Spring + 1, 0), groupSizes, groupSkip, cache);

            // No groups left or group doesn't fit in current spring
            if (groupSizes.Length == groupSkip || groupSize > springs[pointer.Spring].Size - pointer.Index)
                return total;

            // One more group left, but a future spring requires one
            if (groupSizes.Length == groupSkip + 1 && (springs[pointer.Spring].FirstSpringAfter(pointer.Index) > -1 || springs.Skip(pointer.Spring + 1).Any(l => l.RequiresSpring)))
                return total;

            var positions = springs[pointer.Spring].Size - groupSize - pointer.Index;

            // If no more groups left, return amount of options
            if (groupSizes.Length == groupSkip + 1)
                return total + positions + 1;

            for (var i = 0; i <= positions; i++)
            {
                var newPointer = pointer.Move(springs, i + groupSize + 1);
                total += CountPossibleArrangementsWithCache(springs, newPointer, groupSizes, groupSkip + 1, cache);
            }
        }
        // Try fit group before first required spring,
        // and on first required spring
        else
        {
            // Before required spring
            var maxIndex = firstSpring - 1 - groupSize;
            var positions = maxIndex - pointer.Index;

            for (var i = 0; i <= positions; i++)
            {
                var newPointer = pointer.Move(springs, i + groupSize + 1);
                total += CountPossibleArrangementsWithCache(springs, newPointer, groupSizes, groupSkip + 1, cache);
            }

            // On required spring
            maxIndex = Math.Min(firstSpring, springs[pointer.Spring].Size - groupSize);
            var minIndex = Math.Max(pointer.Index, firstSpring + 1 - groupSize);
            for (var i = minIndex; i <= maxIndex; i++)
            {
                // Ignore positions if next to required spring
                if (springs[pointer.Spring].Size > i + groupSize && springs[pointer.Spring].RequiresSpringAt(i + groupSize))
                    continue;

                var newPointer = pointer.Move(springs, i + groupSize + 1 - pointer.Index);
                total += CountPossibleArrangementsWithCache(springs, newPointer, groupSizes, groupSkip + 1, cache);
            }
        }

        return total;
    }
}