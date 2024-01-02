namespace day25;

public record struct Component(string Name, int Value = 1)
{
    public List<Wire> Wires { get; set; } = new List<Wire>();

    public readonly Component Merge(Component other) {
		var merged = new Component("merged", Value + other.Value);
		Rewire(merged, other);
		other.Rewire(merged, this);
		return merged;
    }

    private readonly void Rewire(Component merged, Component exclude) {
        foreach(var wire in Wires)
        {
            if (wire.To == this) {
                if (wire.From != exclude) {
                    wire.To = merged;
                    merged.Wires.Add(wire);
                }
            } else if (wire.From == this) {
                if (wire.To != exclude) {
                    wire.From = merged;
                    merged.Wires.Add(wire);
                }
            }
        }
    }
    
    public readonly bool Connected(Component to, IEnumerable<Wire> without)
	{
        return FindRoute(to, without).Any();
	}
    
    public readonly List<Wire> FindRoute(Component to, IEnumerable<Wire> without)
	{
		var queue  = new List<Route>
        {
            new(this)
        };

		Route? shortestRoute = null;

		var visited = new HashSet<Component>();
        while (queue.Count > 0) {
            var newQueue = new List<Route>();
            foreach (var route in queue) {
                foreach(var wire in route.Component.Wires)
                {
                    if(without.Contains(wire))
                        continue;

                    var other = wire.To == route.Component ? wire.From : wire.To;
                    if(!visited.Add(other))
                        continue;

                    var newRoute = new Route(other, wire, route, route.Length + 1);

                    if(other == to)
                    {
                        if(shortestRoute == null || newRoute.Length < shortestRoute.Length)
                            shortestRoute = newRoute;
                    } else {
                        newQueue.Add(newRoute);
                    }
                }
            }
            queue = newQueue;
        }

        var path = new List<Wire>();
        
        // Found no route
        if (shortestRoute == null)
            return path;

        var previous = shortestRoute;
        for(var i = 0; i < shortestRoute.Length - 1; i++) {
            if (previous?.Wire != null) {
                path.Add(previous.Wire);
                previous = previous.Previous;
            }
        }
        return path;
	}
}
