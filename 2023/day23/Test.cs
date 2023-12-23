using Lib;

namespace day23;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 94)]
    [InlineData("input.txt", 2186)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var start = new Position(1, 0);
        var finish = new Position(input[0].Length - 2, input.Length - 1);
        
        var routes = GetRoutes(input, start, true);
        var best = GetLongestRoute(routes, start, finish);

        Assert.Equal(expectedResult, best);
    }

    [Theory]
    [InlineData("sample.txt", 154)]
    [InlineData("input.txt", 6802)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var start = new Position(1, 0);
        var finish = new Position(input[0].Length - 2, input.Length - 1);

        var routes = GetRoutes(input, start, false);
        var best = GetLongestRoute(routes, start, finish);

        Assert.Equal(expectedResult, best);
    }

    public int GetLongestRoute(IEnumerable<Route> routes, Position start, Position finish) {

        var options = routes.GroupBy(o => o.From).ToDictionary(g => g.Key, g => g.ToArray());
        var queue = new Queue<List<Route>>();
        queue.Enqueue(new List<Route>() { options[start].Single() });
        var validRoutes = new List<List<Route>>();
        while (queue.Count > 0) {
            var route = queue.Dequeue();
            if (route.Last().To == finish)
                validRoutes.Add(route);
            else {
                var optionsFromEnd = options[route.Last().To];
                foreach (var option in optionsFromEnd) {
                    if (!route.Any(r => r.From == option.To))
                        queue.Enqueue(route.Append(option).ToList());
                }
            }
        }
        return validRoutes.Max(r => r.Sum(p => p.Weight));
    }

    private IEnumerable<Route> GetRoutes(string[] map, Position position, bool slipperySlopes)
    {
        var routes = new List<Route>();
        var queue = new Queue<Position>();
        queue.Enqueue(position);

        while (queue.Count > 0) {
            var item = queue.Dequeue();

            // Don't explore routes from end
            if (item.X == map[0].Length - 2 && item.Y == map.Length - 1)
                continue;

            var routesFromPosition = GetRoutesFromPosition(map, item, slipperySlopes);

            if (!routesFromPosition.Any()) {
                throw new Exception("Dead end?");
            }

            foreach (var route in routesFromPosition)
            {
                routes.Add(route);

                // If not visited before, then explore routes from destination
                if (!routes.Any(r => r.From == route.To) && !queue.Contains(route.To))
                    queue.Enqueue(route.To);
            }
        }

        return routes;
    }

    private IEnumerable<Route> GetRoutesFromPosition(string[] map, Position position, bool slipperySlopes) {
        var routes = GetAdjacentOptions(map, position, slipperySlopes).Select(o => new Tuple<Position, Position, int, bool>(position, o, 1, false)).ToArray();
        while (routes.Any(r => !r.Item4)) {
            for (var i = 0; i < routes.Length; i++) {
                var route = routes[i];
                if (!route.Item4) {
                    var options = GetAdjacentOptions(map, route.Item2, slipperySlopes).Where(o => o != route.Item1).ToArray();
                    // Dead end, so mark as finished
                    if (options.Length == 0) {
                        routes[i] = new (route.Item1, route.Item2, route.Item3, true);
                    }
                    // Only one way to go, so continue
                    if (options.Length == 1) {
                        routes[i] = new (route.Item2, options[0], route.Item3 + 1, false);
                    }
                    // Split, so mark as finished
                    else {
                        routes[i] = new (route.Item1, route.Item2, route.Item3, true);
                    }
                }
            }
        }
        return routes.Select(r => new Route(position, r.Item2, r.Item3)).ToArray();
    }

    private IEnumerable<Position> GetAdjacentOptions(string[] map, Position position, bool slipperySlopes) {
        // Start position
        if (position.Y == 0) {
            yield return position with { Y = 1 };
        // Not at the end
        } else if (position.Y < map.Length - 1) {
            var current = map[position.Y][position.X];
            // Remarks:
            // 1.  ^ and < are not in the inputs
            // 2.  There are never two slopes after each other
            // 3.  Slopes never end in a wall
            // 4.  There is a border at the edges
            if (slipperySlopes && current == '>') {
                yield return position with { X = position.X + 1 };
            } else if (slipperySlopes && current == 'v') {
                yield return position with { Y = position.Y + 1 };
            } else if (!slipperySlopes || current == '.') {
                var left = map[position.Y][position.X - 1];
                var right = map[position.Y][position.X + 1];
                var top = map[position.Y - 1][position.X];
                var bottom = map[position.Y + 1][position.X];
                if (left != '#' && (!slipperySlopes || left != '>'))
                    yield return position with { X = position.X - 1 };
                if (right != '#')
                    yield return position with { X = position.X + 1 };
                if (top != '#' && (!slipperySlopes || top != 'v'))
                    yield return position with { Y = position.Y - 1 };
                if (bottom != '#')
                    yield return position with { Y = position.Y+ 1 };
            } else {
                throw new NotImplementedException();
            }
        }
    }
}

public record Position(int X, int Y);
public record Route(Position From, Position To, int Weight);