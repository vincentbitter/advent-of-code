namespace Lib.Extensions;

public static class IntExtensions
{
    public static int Length(this int integer)
    {
        if (integer == 0)
            return 1;
        if (integer < 0)
            return Length(-integer) + 1;

        return (int)Math.Floor(Math.Log10(integer)) + 1;
    }
}