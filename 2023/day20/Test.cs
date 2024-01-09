using Lib;
using Lib.Extensions;

namespace day20;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 32000000)]
    [InlineData("sample2.txt", 11687500)]
    [InlineData("input.txt", 670984704)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var modules = CreateModules(input);
        var broadcaster = modules.Values.Single(m => m is BroadcasterModule);

        for (var i = 0; i < 1000; i++)
            PushButton(broadcaster, 0);

        var totalLow = modules.Values.Sum(m => m.LowSent) + 1000;
        var totalHigh = modules.Values.Sum(m => m.HighSent);
        var result = totalHigh * totalLow;

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("input.txt", 262775362119547)]
    public void PartB(string fileName, long expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var modules = CreateModules(input);

        var xf = modules["xf"];
        var cm = modules["cm"];
        var sz = modules["sz"];
        var gc = modules["gc"];
        var broadcaster = modules.Values.Single(m => m is BroadcasterModule);

        for (var pushes = 1; true; pushes++)
        {
            PushButton(broadcaster, pushes);
            if (xf.RaisedInRun != null && cm.RaisedInRun != null && sz.RaisedInRun != null && gc.RaisedInRun != null)
                break;
        }

        var xf1 = (long)xf.RaisedInRun!.Value;
        var cm1 = cm.RaisedInRun!.Value;
        var sz1 = sz.RaisedInRun!.Value;
        var gc1 = gc.RaisedInRun!.Value;

        var x = xf1.LeastCommonMultiple(cm1)
                .LeastCommonMultiple(sz1)
                .LeastCommonMultiple(gc1);

        Assert.Equal(expectedResult, x);
    }

    private IDictionary<string, Module> CreateModules(string[] input)
    {
        var split = input
            .Select(l => l.Split(" -> "))
            .ToArray();

        var modules = split
            .Select(
                a => CreateModule(a[0])
            ).ToDictionary(m => m.Name, m => m);

        ParseDestinations(split, modules);

        return modules;
    }

    private void ParseDestinations(string[][] input, IDictionary<string, Module> modules)
    {
        foreach (var line in input)
        {
            var sourceName = GetModuleType(line[0]) == ModuleType.Broadcaster ? line[0] : line[0][1..];
            var source = modules[sourceName];
            var destinations = line[1].Split(", ");
            foreach (var destination in destinations)
            {
                modules.TryGetValue(destination, out var module);
                if (module == null)
                    source.FakeDestinations++;
                else
                {
                    module.Sources.Add(source);
                    source.Destinations.Add(module);
                }
            }
        }
    }

    private static void PushButton(Module broadcaster, int pushes)
    {
        var runs = 0;
        var signals = new List<Tuple<Module?, Module>> { new(null, broadcaster) };
        while (signals.Any())
        {
            runs++;
            var newSignals = new List<Tuple<Module?, Module>>();

            foreach (var signal in signals)
            {
                var result = signal.Item2.HandleSignal(signal.Item1!, pushes, runs);
                if (result != null)
                {
                    signal.Item2.State = result.Item2;
                    foreach (var dest in result.Item1)
                    {
                        newSignals.Add(new(signal.Item2, dest));
                    }
                }
            }
            signals = newSignals;
        }
    }

    private ModuleType GetModuleType(string v)
    {
        if (v[0] == '&')
            return ModuleType.Conjunction;
        if (v[0] == '%')
            return ModuleType.FlipFlop;
        return ModuleType.Broadcaster;
    }

    private Module CreateModule(string v)
    {
        var moduleType = GetModuleType(v);
        if (moduleType == ModuleType.Conjunction)
            return new ConjunctionModule(v[1..]);
        else if (moduleType == ModuleType.FlipFlop)
            return new FlipFlopModule(v[1..]);
        return new BroadcasterModule(v);
    }
}
