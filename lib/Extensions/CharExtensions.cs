namespace Lib.Extensions;

public static class CharExtensions
{
    public static bool TryParseInt(this char character, out int value)
    {
        value = character - 48;
        return char.IsDigit(character);
    }
}
