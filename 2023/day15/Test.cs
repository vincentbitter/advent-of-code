using Lib;
using Lib.Extensions;

namespace day15;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 1320)]
    [InlineData("sample2.txt", 52)]
    [InlineData("input.txt", 506891)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllText(fileName);
        var values = input.Split(',').Select(Hash).ToArray();
        var value = values.Sum();
        Assert.Equal(expectedResult, value);
    }

    private int Hash(string arg)
    {
        return arg.Aggregate((byte)0, (t, c) => t = (byte)((t + c) * 17));
    }

    [Theory]
    [InlineData("sample.txt", 145)]
    [InlineData("input.txt", 230462)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllText(fileName);
        var instructions = input.Split(',').ToArray();
        var boxes = Enumerable.Range(0, 256).Select(i => new List<string>()).ToArray();
        foreach (var instruction in instructions)
        {
            // -
            if (instruction[^1] == '-')
            {
                var group = instruction[..^1];
                var label = Hash(group);
                var box = boxes[label];
                boxes[label] = box.Where(v => v[..^2] != group).ToList();
            }

            // =
            else
            {
                var group = instruction[..^2];
                var label = Hash(group);
                var box = boxes[label];

                var existing = box.FirstOrDefault(v => v[..^2] == group);
                if (existing != null)
                {
                    var i = box.IndexOf(existing);
                    box.Remove(existing);
                    box.Insert(i, instruction);
                }
                else
                    box.Add(instruction);
            }
        }

        var total = 0;
        for (var i = 0; i < boxes.Length; i++)
        {
            var box = boxes[i];
            for (var j = 0; j < box.Count; j++)
            {
                var value = box[j][^1].ToInt();
                var subTotal = (i + 1) * (j + 1) * value;
                total += subTotal;
            }
        }

        Assert.Equal(expectedResult, total);
    }
}