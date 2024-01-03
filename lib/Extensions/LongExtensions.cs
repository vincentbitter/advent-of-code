namespace Lib.Extensions;

public static class LongExtensions
{
    public static long GreatestCommonDivisor(this long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public static long LeastCommonMultiple(this long a, long b)
    {
        return a / GreatestCommonDivisor(a, b) * b;
    }
}