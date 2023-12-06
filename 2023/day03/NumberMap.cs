namespace day03;

public class NumberMap
{
    private string[] _lines;

    public NumberMap(string[] lines) {
        _lines = lines;
    }

    public IEnumerable<Number> GetAllNumbers()
    {
        for (var l = 0; l < _lines.Length; l++) {
            var line = _lines[l];
            for(var i = 0; i < line.Length; i++) {
                if (char.IsDigit(line[i])) {
                    var indexFrom = i;
                    var chars = "" + line[i];
                    while (line.Length > i + 1 && char.IsDigit(line[i+1])) {
                        i++;
                        chars += line[i];
                    }
                    yield return new Number(indexFrom, l, chars.Length, int.Parse(chars));
                }
            }
        }
    }

    public IEnumerable<Symbol> GetAllSymbols()
    {
        for (var l = 0; l < _lines.Length; l++) {
            var line = _lines[l];
            for(var i = 0; i < line.Length; i++) {
                if (IsSymbol(line[i])) {
                    yield return new Symbol(i, l, line[i]);
                }
            }
        }
    }

    private bool IsSymbol(char c) {
        return !char.IsDigit(c) && c != '.';
    }
}