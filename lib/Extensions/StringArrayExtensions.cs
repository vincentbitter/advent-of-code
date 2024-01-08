namespace Lib.Extensions;

public static class StringArrayExtensions
{
    public static string[] RotateClockwise(this string[] source)
    {
        var dest = new string[source[0].Length];
        for (var row = source.Length - 1; row >= 0; row--)
            for (var column = 0; column < source[0].Length; column++)
                dest[column] += source[row][column];
        return dest;
    }

    public static string[] RotateCounterClockwise(this string[] source)
    {
        var dest = new string[source[0].Length];
        for (var column = source[0].Length - 1; column >= 0; column--)
            for (var row = 0; row < source.Length; row++)
                dest[source[0].Length - column - 1] += source[row][column];
        return dest;
    }
}