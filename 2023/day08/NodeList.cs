namespace day08;

public class NodeList : Dictionary<string, Node>
{
    private bool _singleStartAndEndNode;

    public NodeList(IEnumerable<string> lines, bool singleStartAndEndNode)
    {
        _singleStartAndEndNode = singleStartAndEndNode;

        foreach (var line in lines)
        {
            var name = line.Substring(0, 3);
            var nameLeft = line.Substring(7, 3);
            var nameRight = line.Substring(12, 3);

            var node = GetOrCreateNode(name);
            node.Left = GetOrCreateNode(nameLeft);
            node.Right = GetOrCreateNode(nameRight);
        }
    }

    internal IEnumerable<Node> GetStartNodes()
    {
        return Values.Where(n => n.IsStartNode).ToArray();
    }

    private Node GetOrCreateNode(string name)
    {
        if (!TryGetValue(name, out var node))
        {
            node = new Node(name, _singleStartAndEndNode);
            Add(name, node);
        }
        return node;
    }
}
