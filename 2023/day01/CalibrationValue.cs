using Lib.Extensions;

namespace day01;

public readonly struct CalibrationValue
{
    private static readonly string[] _numbers = new[] { "zero", "one", "two",
        "three", "four", "five", "six", "seven", "eight", "nine" };

    private readonly string _line;

    public CalibrationValue(string line)
    {
        _line = line;
    }

    public readonly int GetValue(bool alsoTextual = false)
    {
        return GetFirstDigit(alsoTextual) * 10 + GetLastDigit(alsoTextual);
    }

    private readonly int GetFirstDigit(bool alsoTextual = false)
    {
        for (var p = 0; p < _line.Length; p++)
        {
            if (TryReadDigit(p, out var value))
                return value;
            if (alsoTextual && TryReadTextDigit(p, out value))
                return value;
        }

        throw new ArgumentOutOfRangeException("line", "String does not contain a number");
    }

    private readonly int GetLastDigit(bool alsoTextual = false)
    {
        for (var p = _line.Length - 1; p >= 0; p--)
        {
            if (TryReadDigit(p, out var value))
                return value;
            if (alsoTextual && TryReadTextDigit(p, out value))
                return value;
        }

        throw new ArgumentOutOfRangeException("line", "String does not contain a number");
    }

    private readonly bool TryReadDigit(int position, out int value)
    {
        return _line[position].TryParseInt(out value);
    }

    private readonly bool TryReadTextDigit(int position, out int value)
    {
        for (var i = 1; i < _numbers.Length; i++)
        {
            if (_numbers[i].Length + position <= _line.Length &&
                    _line.Substring(position, _numbers[i].Length) == _numbers[i])
            {
                value = i;
                return true;
            }
        }

        value = 0;
        return false;
    }
}