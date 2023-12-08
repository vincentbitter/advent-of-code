using Lib;

namespace day08;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 2)]
    [InlineData("sample2.txt", 6)]
    [InlineData("input.txt", 21883)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var directions = input[0];
        var nodes = input.Skip(2).Select(l => l.Split(" = "))
            .ToDictionary(l => l[0], l => new Tuple<string, string>(l[1].Substring(1, 3), l[1].Substring(6, 3)));

        var currentNode = "AAA";
        var steps = 0;
        while (currentNode != "ZZZ")
        {
            currentNode = directions[steps % directions.Length] == 'L' ? nodes[currentNode].Item1 : nodes[currentNode].Item2;
            steps++;
        }

        Assert.Equal(expectedResult, steps);
    }

    [Theory]
    [InlineData("sample3.txt", 6)]
    [InlineData("input.txt", 12833235391111)]
    public void PartB(string fileName, long expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var directions = input[0];
        var nodes = input.Skip(2).Select(l => l.Split(" = "))
            .ToDictionary(l => l[0], l => new Tuple<string, string>(l[1].Substring(1, 3), l[1].Substring(6, 3)));

        
        var keyNodes = nodes.Keys.Where(n => n.EndsWith('A') || n.EndsWith('Z')).ToArray();
        var stepsToNextNode = new Dictionary<string, int[]>();
        var nextNode = new Dictionary<string, string[]>();
        foreach (var node in keyNodes)
        {
            stepsToNextNode[node] = new int[directions.Length + 1];
            nextNode[node] = new string[directions.Length + 1];
            for (var offset = 0; offset < directions.Length; offset++)
            {
                var stepsToZ = 0;
                var currentNode = node;
                while (!currentNode.EndsWith('Z') || stepsToZ == 0)
                {
                    currentNode = directions[stepsToZ % directions.Length] == 'L' ? nodes[currentNode].Item1 : nodes[currentNode].Item2;
                    stepsToZ++;
                }
                nextNode[node][offset] = currentNode;
                stepsToNextNode[node][offset] = stepsToZ;
            }
        }
        
        var currentNodes = nodes.Keys.Where(n => n.EndsWith('A')).ToArray();
        var currentSteps = currentNodes.Select(s => 0L).ToArray();

        while (currentSteps.Distinct().Count() > 1 || currentSteps[0] == 0) {
            var max = currentSteps.Max();
            for (var i = 0; i < currentSteps.Length; i++) {
                if (currentSteps[i] < max || max == 0) {
                    var offset = unchecked((int)(currentSteps[i] % directions.Length));
                    currentSteps[i] += stepsToNextNode[currentNodes[i]][offset];
                    currentNodes[i] = nextNode[currentNodes[i]][offset];
                }
            }
        }

        Assert.Equal(expectedResult, currentSteps[0]);
    }
}

public record Node(string name, string left, string right);