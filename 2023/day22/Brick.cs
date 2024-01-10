using Lib.Geometry;
using Lib.Misc;

namespace day22;

public record Brick : Object3D
{
    public int Id { get; init; }
    public List<Brick> SupportedBy { get; set; } = new List<Brick>();
    public List<Brick> Supporting { get; set; } = new List<Brick>();

    private static int _lastId = 0;

    public Brick(Point3D From, Point3D To) : base(From, To)
    {
        _lastId++;
        Id = _lastId;
    }

    public int GetDamage()
    {
        var removed = new HashSet<int> { Id };
        QueueLoop.Run(Supporting, (brick) =>
        {
            if (!brick.SupportedBy.Any(b => !removed.Contains(b.Id)))
            {
                if (removed.Add(brick.Id))
                    return brick.Supporting.Where(b => !removed.Contains(b.Id));
            }
            return null;
        });
        return removed.Count - 1;
    }

    public bool IsSingleSupporter()
    {
        return Supporting.Any(b => b.SupportedBy.Count == 1);
    }
}