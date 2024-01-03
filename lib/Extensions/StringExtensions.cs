namespace Lib.Extensions;

public static class StringExtensions
{
    public static IEnumerable<int> AllIndexesOf(this string str, char value)
    {
        int minIndex = str.IndexOf(value);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = str.IndexOf(value, minIndex + 1);
        }
    }
}