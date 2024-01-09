namespace day20;

public class ConjunctionModule : Module
{
    public ConjunctionModule(string name) : base(name)
    {
    }

    public override Tuple<IEnumerable<Module>, bool> HandleSignal(Module source, int pushes, int runs)
    {
        var newState = Sources.Any(s => !s.State);
        if (newState)
        {
            if (newState != State && RaisedInRun == null)
                RaisedInRun = pushes;
            HighSent += Destinations.Count + FakeDestinations;
        }
        else
        {
            LowSent += Destinations.Count + FakeDestinations;
        }
        return new(Destinations, newState);
    }
}