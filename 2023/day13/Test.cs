using Lib;

namespace day13;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 405)]
    [InlineData("sample2.txt", 2)]
    [InlineData("input.txt", 39939)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var inputs = ParseInput(input);
        
        var verticalCount = 0;
        var horizontalCount = 0;
        foreach(var map in inputs) {
            var rotatedMap = RotateRight(map);
            var h = MirroredRowsScore(map);
            var v = MirroredRowsScore(rotatedMap);
            if (h > v)
                horizontalCount += h;
            else
                verticalCount += v;
        }
        
        Assert.Equal(expectedResult, (horizontalCount * 100) + verticalCount);
    }

    [Theory]
    [InlineData("sample.txt", 400)]
    [InlineData("input.txt", 32069)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var inputs = ParseInput(input);
        
        var verticalCount = 0;
        var horizontalCount = 0;
        foreach(var map in inputs) {
            var rotatedMap = RotateRight(map);
            var h = MirroredRowsScore(map);
            var v = MirroredRowsScore(rotatedMap);
            
            var h2 = MirroredRowScoreWithRepair(map, h - 1);
            var v2 = MirroredRowScoreWithRepair(rotatedMap, v - 1);

            if (h2 != h && h2 > 0)
                horizontalCount += h2;
            else if (v2 != v && v2 > 0)
                verticalCount += v2;
            else
                Assert.Fail("Error");
        }
        
        Assert.Equal(expectedResult, (horizontalCount * 100) + verticalCount);
    }

    private List<string[]> ParseInput(string[] input) {
        var inputs = new List<string[]>();
        var start = 0;
        for (var row = 0; row < input.Length; row++) {
            if (input[row] == "") {
                inputs.Add(input.Skip(start).Take(row - start).ToArray());
                start = row + 1;
            }
        }
        inputs.Add(input.Skip(start).Take(input.Length - start).ToArray());
        return inputs;
    }

    private string[] RotateRight(string[] map) {
        var rotatedInput = new string[map[0].Length];
        for (var row = map.Length - 1; row >= 0 ; row--)
            for (var column = 0; column < map[0].Length; column++)
                rotatedInput[column] += map[row][column];
        return rotatedInput;
    }

    private int MirroredRowsScore(string[] map, int skipIndex = -1) {
        var centers = FindCenterIndexes(map);
        foreach (var center in centers) {
            if (center == skipIndex)
                continue;

            var i = center;
            var j = center + 1;
            while (i >= 0 && j < map.Length && map[i] == map[j]) {
                i--;
                j++;
            }

            var max = 0;
            if (center < map.Length / 2)
                max = (center + 1) * 2;
            else
                max = (map.Length - center - 1) * 2;
            var rows = j - i - 1;
            if (max == rows)
                return center + 1;
        }
        return 0;
    }

    private int[] FindCenterIndexes(string[] map) {
        var centers = new List<int>();
        for (var i = 1; i < map.Length; i++) {
            if (map[i] == map[i-1]) {
                centers.Add(i - 1);
            }
        }
        return centers.ToArray();
    }

    private int MirroredRowScoreWithRepair(string[] map, int skipIndex)
    {
        var top = 0;
        for (var row = 0; row < map.Length; row++) {
            for (var column = 0; column < map[0].Length; column++) {
                var newMap = map.ToArray();
                newMap[row] = newMap[row].Substring(0, column) 
                    + (map[row][column] == '.' ? '#' : '.')
                    + newMap[row].Substring(column + 1);
                var score = MirroredRowsScore(newMap, skipIndex);
                if (score > top)
                    top = score;
            }
        }
        return top;
    }
}