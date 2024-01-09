namespace day20;

public class BroadcasterModule : Module
{
    public BroadcasterModule(string name) : base(name)
    {
    }

    public override Tuple<IEnumerable<Module>, bool> HandleSignal(Module source, int pushes, int runs)
    {
        LowSent += Destinations.Count + FakeDestinations;
        return new(Destinations, false);
    }
}
