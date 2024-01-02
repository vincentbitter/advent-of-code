using Lib;

namespace day25;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 54)]
    [InlineData("input.txt", 507626)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var sets = input.Select(line => line.Split(": ")).ToDictionary(line => line[0], line => line[1].Split(' '));

        var wireSets = sets.SelectMany(s => s.Value.Select(v => new Tuple<string, string>(s.Key, v))).ToList();
        var bidirectionalWires = wireSets.Concat(wireSets.Select(w => new Tuple<string, string>(w.Item2, w.Item1)));

        var components = bidirectionalWires.Select(w => w.Item1)
            .Distinct().Select(name => new Component(name)).ToList();

        foreach (var wireSet in wireSets)
        {
            var from = components.Single(n => n.Name == wireSet.Item1);
            var to = components.Single(n => n.Name == wireSet.Item2);
            var wire = new Wire
            {
                From = from,
                To = to
            };
            from.Wires.Add(wire);
            to.Wires.Add(wire);
        }
        components = CutInTwo(components);
        var r = components[0].Value * components[1].Value;
        Assert.Equal(expectedResult, r);
    }

    public static List<Component> CutInTwo(List<Component> components)
    {
        while (components.Count > 2)
        {
            foreach (var component in components)
            {
                var wire = component.Wires.FirstOrDefault(wire =>
                {
                    // Nodes with less than three wires can never have three unique routes
                    if (wire.From.Wires.Count <= 3 || wire.To.Wires.Count <= 3)
                        return false;

                    // Try remove three different routes
                    var without = new List<Wire>() { wire };
                    for (int k = 1; k < 3; k++)
                        without.AddRange(wire.From.FindRoute(wire.To, without));

                    // Check if still is connected
                    return wire.From.Connected(wire.To, without);
                });

                if (wire != null)
                {
                    var other = wire.From == component
                        ? wire.To
                        : wire.From;
                    components.Remove(wire.To);
                    components.Remove(wire.From);
                    components.Add(component.Merge(other));

                    break;
                }
            }
        }
        return components;
    }
}