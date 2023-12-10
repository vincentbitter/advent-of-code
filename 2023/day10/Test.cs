using Lib;

namespace day10;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 8, 'F')]
    [InlineData("input.txt", 6697, 'J')]
    public void PartA(string fileName, int expectedResult, char startingPositionChar)
    {
        var input = Parser.ReadAllLines(fileName);
        var maxX = input[0].Length;
        var startLine = input.Single(line => line.Contains('S'));
        var startY = input.ToList().IndexOf(startLine);
        var startX = startLine.IndexOf('S');

        var count = 0;
        var previous = new Tuple<int,int>(startX,startY);
        var position = new Tuple<int,int>(startX,startY);
        var curentChar = startingPositionChar;
        while (curentChar != 'S') {
            var newPosition = GetNextPosition(curentChar, previous, position);
            previous = position;
            position = newPosition;
            curentChar = GetChar(input, position);
            count++;
        }
        Assert.Equal(expectedResult, count / 2);
    }

    [Theory]
    [InlineData("sample.txt", 1, 'F')]
    [InlineData("sample2.txt", 10, 'F')]
    [InlineData("sample3.txt", 4, 'F')]
    [InlineData("input.txt", 423, 'J')]
    public void PartB(string fileName, int expectedResult, char startingPositionChar)
    {
        var input = Parser.ReadAllLines(fileName);
        var input2 = new string[input.Length + 2];
        input2[0] = "".PadLeft(input[0].Length + 2, '.');
        input2[input2.Length - 1] = input2[0];
        for(var y = 0; y < input.Length; y++)
            input2[y+1] = '.' + input[y] + '.';
        input = input2;

        var maxX = input[0].Length;
        var startLine = input.Single(line => line.Contains('S'));
        var startY = input.ToList().IndexOf(startLine);
        var startX = startLine.IndexOf('S');

        var previous = new Tuple<int,int>(startX,startY);
        var position = new Tuple<int,int>(startX,startY);
        var curentChar = startingPositionChar;
        var visited = new List<Tuple<int,int>>();
        while (curentChar != 'S') {
            var newPosition = GetNextPosition(curentChar, previous, position);
            previous = position;
            position = newPosition;
            curentChar = GetChar(input, position);
            visited.Add(position);
        }

        //var count = 0;
        var newInput = new string[input.Length * 2 - 1];
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < maxX; x++) {
                var charachter = input[y][x];
                if (charachter == 'S')
                    charachter = startingPositionChar;
                if (visited.Any(v => v.Item1 == x && v.Item2 == y)) {
                    if (charachter == 'F' || charachter == 'L' || charachter == '-')
                        newInput[y*2] += charachter + "-";
                    else if (charachter == 'J' || charachter == '7' || charachter == '|')
                        newInput[y*2] += charachter + " ";
                    
                    if (y < input.Length - 1) {
                        if (charachter == 'F' || charachter == '7' || charachter == '|')
                            newInput[y*2+1] += "| ";
                        else if (charachter == 'J' || charachter == 'L' || charachter == '-')
                            newInput[y*2+1] += "  ";
                    }
                }
                else {
                    newInput[y*2] += ". ";
                    if (y < input.Length - 1)
                        newInput[y*2+1] += "  ";
                }
            }
        }

        var count = 0;
        var itemsToExclude = GetIemsToExclude(newInput, 0, 0);
        for (var y = 0; y < newInput.Length; y++) {
            for (var x = 0; x < newInput[0].Length; x++) {
                if (newInput[y][x] == '.' && !itemsToExclude.Contains(new Tuple<int, int>(x,y)))
                    count++;
            }
        }

        Assert.Equal(expectedResult, count);
    }

    private Tuple<int,int> GetNextPosition(char current, Tuple<int,int> previous, Tuple<int,int> position) {
        if (current == '|') {
            var toX = position.Item1;
            var toY = 2 * position.Item2 - previous.Item2;
            return new Tuple<int,int>(toX,toY);
        }
        if (current == '-') {
            var toX = 2 * position.Item1 - previous.Item1;
            var toY = position.Item2;
            return new Tuple<int,int>(toX,toY);
        }
        if (current == 'L') {
            if (position.Item2 > previous.Item2)
                return new Tuple<int, int>(position.Item1 + 1, position.Item2); // Right
            return new Tuple<int, int>(position.Item1, position.Item2 - 1); // Up
        }
        if (current == 'J') {
            if (position.Item2 > previous.Item2)
                return new Tuple<int, int>(position.Item1 - 1, position.Item2); // Left
            return new Tuple<int, int>(position.Item1, position.Item2 - 1); // Up
        }
        if (current == '7') {
            if (position.Item2 < previous.Item2)
                return new Tuple<int, int>(position.Item1 - 1, position.Item2); // Left
            return new Tuple<int, int>(position.Item1, position.Item2 + 1); // Down
        }
        if (current == 'F') {
            if (position.Item2 < previous.Item2)
                return new Tuple<int, int>(position.Item1 + 1, position.Item2); // Right
            return new Tuple<int, int>(position.Item1, position.Item2 + 1); // Down
        }
        if (current == '.') {
            throw new NotImplementedException("Landed on the ground!");
        }
        throw new NotImplementedException("Don't recognize: " + current);
    }

    private char GetChar(string[] map, Tuple<int,int> position) {
        return map[position.Item2][position.Item1];
    }

    private List<Tuple<int,int>> _next = new List<Tuple<int,int>>();
    private List<Tuple<int,int>> _visited = new List<Tuple<int,int>>();

    private IEnumerable<Tuple<int,int>> X(string[] input) {
        _visited.AddRange(_next);
        var items = _next.ToList();
        _next.Clear();
        foreach (var p in items) {
            var x = p.Item1;
            var y = p.Item2;
            if (input[y][x] == '.' || input[y][x] == ' ') {
                yield return new Tuple<int, int>(x,y);
                if (x < input[y].Length - 1) {
                    _next.Add(new Tuple<int, int>(x + 1, y));
                }
                if (y < input.Length - 1) {
                    _next.Add(new Tuple<int, int>(x, y + 1));
                }
                if (x > 0) {
                    _next.Add(new Tuple<int, int>(x - 1, y));
                }
                if (y > 0) {
                    _next.Add(new Tuple<int, int>(x, y - 1));
                }
            }
        }
        _next = _next.Except(_visited).ToList();
    }

    private IEnumerable<Tuple<int,int>> GetIemsToExclude(string[] input, int x, int y) {
        _visited.Add(new Tuple<int, int>(x, y));
        _next.Add(new Tuple<int, int>(x + 1, y));
        _next.Add(new Tuple<int, int>(x, y + 1));
        var toExclude = new List<Tuple<int,int>>();
        toExclude.Add(new Tuple<int, int>(x,y));
        while (_next.Any()) {
            toExclude.AddRange(X(input));
        }
        return toExclude;
    }

    private bool CanEscape(string[] input, string[] rotatedInput, int x, int y) {
        // Escape to left
        if (input[y].Substring(0, x).All(c => c == '.' || c == ' '))
            return true;
        // Escape to right
        if (input[y].Substring(x).All(c => c == '.' || c == ' '))
            return true;
        // Escape to top
        if (rotatedInput[x].Substring(0, y).All(c => c == '.' || c == ' '))
            return true;
        // Escape to bottom
        if (rotatedInput[x].Substring(y).All(c => c == '.' || c == ' '))
            return true;

        return false;
    }
}
