using Lib;
using Lib.Extensions;

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
        var nodes = new NodeList(input.Skip(2), true);

        var currentNode = nodes.GetStartNodes().Single();
        var steps = currentNode.GetDistanceToEndNode(directions, 0);

        Assert.Equal(expectedResult, steps);
    }

    [Theory]
    [InlineData("sample3.txt", 6)]
    [InlineData("input.txt", 12833235391111)]
    public void PartB(string fileName, long expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var directions = input[0];
        var nodes = new NodeList(input.Skip(2), false);

        var keyNodes = nodes.GetStartNodes();
        var distanceToZ = keyNodes.Select(n => n.GetDistanceToEndNode(directions, 0)).ToArray();
        var result = distanceToZ.Aggregate(1L, (value, distance) => value.LeastCommonMultiple(distance));

        Assert.Equal(expectedResult, result);
    }
}