namespace Lib.Extensions;

public static class ArrayExtensions
{
    public static T[][] SplitByValue<T>(this T[] source, T value) where T : class
    {
        var sets = new List<T[]>();
        var start = 0;

        do
        {
            var end = Array.IndexOf(source, value, start + 1);
            var set = source.Skip(start);
            if (end > 0)
                set = set.Take(end - start);
            sets.Add(set.ToArray());
            start = end + 1;
        } while (start > 0);
        
        return sets.ToArray();
    }
}