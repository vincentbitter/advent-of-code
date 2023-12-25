namespace day25;

public record struct Component(string Name, int Value = 1)
{
    public List<Wire> Wires { get; set; } = new List<Wire>();

    public Component Merge(Component other) {
		var merged = new Component("merged", Value + other.Value);
		Rewire(merged, other);
		other.Rewire(merged, this);
		return merged;
    }

    private void Rewire(Component merged, Component exclude) {
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
    
    public bool Connected(Component to, IEnumerable<Wire> without)
	{
        return FindRoute(to, without) != null;
	}
    
    public List<Wire> FindRoute(Component to, IEnumerable<Wire> without)
	{
		var queue  = new List<Route>();
		queue.Add(new Route(this));

		var bestScore = int.MaxValue;
		Route shortestRoute = null;

		var scores = new Dictionary<Component, int>();
        while (queue.Count > 0) {
            var newQueue = new List<Route>();
            foreach (var route in queue) {
                foreach(var wire in route.Component.Wires)
                {
                    if(without.Contains(wire)) continue;

                    var other = wire.To == route.Component ? wire.From : wire.To;
                    if(scores.TryGetValue(other, out var score) && score <= route.Length + 1)
                        continue;

                    var newRoute = new Route(other, wire, route, route.Length + 1);
                    scores[other] = newRoute.Length;

                    if(other == to)
                    {
                        if(newRoute.Length < bestScore)
                        {
                            bestScore = newRoute.Length;
                            shortestRoute = newRoute;
                        }
                    } else {
                        newQueue.Add(newRoute);
                    }
                }
            }
            queue = newQueue;
        }

        // Found no route
        if (shortestRoute == null)
            return null;

        var path = new List<Wire>();
        var previous = shortestRoute;
        for(var i = 0; i < shortestRoute.Length; i++) {
            path.Add(previous.Wire);
            previous = previous.Previous;
        }
        return path;
	}
}
