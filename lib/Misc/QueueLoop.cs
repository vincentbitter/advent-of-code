namespace Lib.Misc;

public static class QueueLoop
{
    public static void Run<T>(IEnumerable<T> init, Func<T, IEnumerable<T>?> func)
    {
        var queue = new HashSet<T>(init);
        while (queue.Count > 0)
        {
            queue = queue.Select(item => func(item))
                .Where(r => r != null)
                .SelectMany(r => r!)
                .ToHashSet();
        }
    }
}