namespace day08;

public record Node
{
    public string Name { get; init; }
    public Node? Left { get; set; }
    public Node? Right { get; set; }

    public bool IsStartNode { get; init; }
    public bool IsEndNode { get; init; }

    public Node(string name, bool singleStartAndEndNode)
    {
        Name = name;
        IsStartNode = singleStartAndEndNode ? Name == "AAA" : Name.EndsWith('A');
        IsEndNode = singleStartAndEndNode ? Name == "ZZZ" : Name.EndsWith('Z');
    }

    public int GetDistanceToEndNode(string directions, int offset)
    {
        var distance = 0;
        var currentNode = this;
        do
        {
            var index = (offset + distance) % directions.Length;
            currentNode = directions[index] == 'L' ? currentNode.Left : currentNode.Right;
            distance++;
        } while (currentNode != null && !currentNode.IsEndNode);

        return distance;
    }
}