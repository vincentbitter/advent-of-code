using Lib;

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
        var modules = input
            .Select(l => l.Split(" -> "))
            .Select(
                a => CreateModule(a[0])
            ).ToList();

        foreach (var line in input)
        {
            var p = line.Split(" -> ");
            var sourceName = GetModuleType(p[0]) == ModuleType.Broadcaster ? p[0] : p[0].Substring(1);
            var source = modules.Single(m => m.Name == sourceName);
            var destinations = p[1].Split(", ");
            foreach (var destination in destinations) {
                var m = modules.SingleOrDefault(m => m.Name == destination);
                if (m == null)
                    source.FakeDestinations++;
                else
                {
                    m.Sources.Add(source);
                    source.Destinations.Add(m);
                }
            }
        }

        for(var i = 0; i < 1000; i++)
            PushButton(modules, 0);

        var totalLow = modules.Sum(m => m.LowSent) + 1000;
        var totalHigh = modules.Sum(m => m.HighSent);
        var result = totalHigh * totalLow;

        Assert.Equal(expectedResult, result);
    }

    public void PushButton(List<Module> modules, long pushes) {
        var broadcaster = modules.Single(m => m is BroadcasterModule);
        //var text = new List<string>();
        var runs = 0L;
        var signals = new List<Tuple<Module,Module>> { new (null, broadcaster) };
        while (signals.Any()) {
            runs++;
            var newSignals = new List<Tuple<Module,Module>>();
            var newStates = new Dictionary<Module, bool>();
            foreach (var signal in signals) {
                var result = signal.Item2.HandleSignal(signal.Item1, pushes, runs);
                if (result != null) {
                    signal.Item2.State = result.Item2;
                    foreach (var dest in result.Item1) {
                        newSignals.Add(new(signal.Item2, dest));
                    }
                    //text.Add(signal.Item2.Name + " -" + (result.Item2 ? "high" : "low") + "-> " + string.Join(',', signal.Item2.Destinations.Select(d => d.Name)));
                }
            }
            signals = newSignals.ToList();
        }
    }

    private ModuleType GetModuleType(string v)
    {
        if (v.StartsWith('&'))
            return ModuleType.Conjunction;
        if (v.StartsWith('%'))
            return ModuleType.FlipFlop;
        return ModuleType.Broadcaster;
    }

    private Module CreateModule(string v)
    {
        if (v.StartsWith('&'))
            return new ConjunctionModule {
                Name = v.Substring(1)
            };
        if (v.StartsWith('%'))
            return new FlipFlopModule() {
                Name = v.Substring(1)
            };
        return new BroadcasterModule() {
                Name = v
            };
    }

    [Theory]
    [InlineData("input.txt", "zr", 262775362119547)]
    public void PartB(string fileName, string senderToRx, long expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var modules = input
            .Select(l => l.Split(" -> "))
            .Select(
                a => CreateModule(a[0])
            ).ToList();

        foreach (var line in input)
        {
            var p = line.Split(" -> ");
            var sourceName = GetModuleType(p[0]) == ModuleType.Broadcaster ? p[0] : p[0].Substring(1);
            var source = modules.Single(m => m.Name == sourceName);
            var destinations = p[1].Split(", ");
            foreach (var destination in destinations) {
                var m = modules.SingleOrDefault(m => m.Name == destination);
                if (m == null)
                    source.FakeDestinations++;
                else
                {
                    m.Sources.Add(source);
                    source.Destinations.Add(m);
                }
            }
        }

        var rxSender = modules.Single(m => m.Name == senderToRx);

        var pushes = 0;
        var xf = modules.SingleOrDefault(m => m.Name == "xf");
        var cm = modules.SingleOrDefault(m => m.Name == "cm");
        var sz = modules.SingleOrDefault(m => m.Name == "sz");
        var gc = modules.SingleOrDefault(m => m.Name == "gc");

        var runs = 0L;
        for (var i = 0L; i < 4497522670; i++) {
            pushes++;
            PushButton(modules, pushes);
            if (xf.HighSent > 0 && cm.HighSent > 0 && sz.HighSent > 0 && gc.HighSent > 0)
                break;
        }
        

        var xf1 = xf.RaisedInRun.First().Item1;
        var cm1 = cm.RaisedInRun.First().Item1;
        var sz1 = sz.RaisedInRun.First().Item1;
        var gc1 = gc.RaisedInRun.First().Item1;

        var x = lcm(xf1, cm1);
        x = lcm(x, sz1);
        x = lcm(x, gc1);
        
        Assert.Equal(expectedResult, x);
    }

    static long gcf(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long lcm(long a, long b)
    {
        return (a / gcf(a, b)) * b;
    }
}

public enum ModuleType {
    FlipFlop,
    Conjunction,
    Broadcaster
}

public abstract class Module {
    public long LowSent {get;set;}
    public long HighSent {get;set;}

    public string Name {get;set;}

    public List<Module> Sources {get;set;} = new List<Module>();
    public List<Module> Destinations {get;set;} = new List<Module>();

    public bool State {get;set;}
    public int FakeDestinations { get; set; }

    public List<Tuple<long,long>> RaisedInRun = new List<Tuple<long,long>>();
    public List<Tuple<long,long>> LoweredInRun = new List<Tuple<long,long>>();

    public abstract Tuple<IEnumerable<Module>, bool> HandleSignal(Module source, long pushes, long runs);
}

public class BroadcasterModule : Module
{
    public override Tuple<IEnumerable<Module>, bool> HandleSignal(Module source, long pushes, long runs) {
        LowSent += Destinations.Count + FakeDestinations;
        return new(Destinations, false);
    }
}

public class ConjunctionModule : Module
{
    public override Tuple<IEnumerable<Module>, bool> HandleSignal(Module source, long pushes, long runs) {
        var newState = Sources.Any(s => !s.State);
        if (newState)
        {
            if (newState != State)
                RaisedInRun.Add(new(pushes,runs));
            HighSent += Destinations.Count + FakeDestinations;
        }
        else {
            if (newState != State)
                LoweredInRun.Add(new(pushes,runs));
            LowSent += Destinations.Count + FakeDestinations;
        }
        return new (Destinations, newState);
    }
}

public class FlipFlopModule : Module
{
    public override Tuple<IEnumerable<Module>, bool> HandleSignal(Module source, long pushes, long runs) {
        if (!source.State) {
            var newState = !State;
            if (newState)
                HighSent += Destinations.Count + FakeDestinations;
            else
                LowSent += Destinations.Count + FakeDestinations;
            return new(Destinations, newState);    
        }

        return null;
    }
}