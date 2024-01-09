namespace day20;

public abstract class Module
{
    public long LowSent { get; set; }
    public long HighSent { get; set; }

    public string Name { get; set; }

    public List<Module> Sources { get; set; } = new List<Module>();
    public List<Module> Destinations { get; set; } = new List<Module>();

    public bool State { get; set; }
    public int FakeDestinations { get; set; }

    public int? RaisedInRun { get; protected set; }

    public abstract Tuple<IEnumerable<Module>, bool> HandleSignal(Module source, int pushes, int runs);

    public Module(string name)
    {
        Name = name;
    }
}
