namespace day20;

public class FlipFlopModule : Module
{
    public FlipFlopModule(string name) : base(name)
    {
    }

    public override Tuple<IEnumerable<Module>, bool> HandleSignal(Module source, int pushes, int runs)
    {
        if (!source.State)
        {
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