using Lib;

namespace day18;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 62, false)]
    [InlineData("sample2.txt", 37, false)]
    [InlineData("input.txt", 35991, false)]
    [InlineData("sample.txt", 952408144115, true)]
    [InlineData("input.txt", 54058824661845, true)]
    public void PartAB(string fileName, long expectedResult, bool useHex)
    {
        var input = Parser.ReadAllLines(fileName);
        var currentX = 0;
        var currentY = 0;
        var horizontalLines = new List<Tuple<int,int,int>>();
        var verticalLines = new List<Tuple<int,int,int>>();
        foreach(var line in input) {
            var parts = line.Split(' ');
            var direction = useHex ? parts[2][7] : parts[0][0];
            var length = useHex ? Convert.ToInt32("0x" + parts[2].Substring(2, 5), 16) : int.Parse(parts[1]);
            
            if(direction == 'D' || direction == '1') {
                verticalLines.Add(new(currentX, currentY, length + 1));
                currentY += length;
            } else if(direction == 'U' || direction == '3') {
                currentY -= length;
                verticalLines.Add(new(currentX, currentY, length + 1));
            } else if(direction == 'L' || direction == '2') {
                currentX -= length;
                horizontalLines.Add(new(currentX, currentY, length + 1));
            } else if(direction == 'R' || direction == '0') {
                horizontalLines.Add(new(currentX, currentY, length + 1));
                currentX += length;
            }
        }

        var minX = horizontalLines.Min(c => c.Item1);
        var maxX = horizontalLines.Max(c => c.Item1 + c.Item3) - 1;
        var minY = verticalLines.Min(c => c.Item2);
        var maxY = verticalLines.Max(c => c.Item2 + c.Item3) - 1;


        var additionalLines = new List<Tuple<int,int,int>>();
        var horizontalLinesPerY = horizontalLines.GroupBy(l => l.Item2).ToDictionary(g => g.Key, g => g.ToList());
        for (var y = minY; y <= maxY; y++) {
            var lines = verticalLines.Where(l => l.Item2 <= y && l.Item2 + l.Item3 > y).OrderBy(l => l.Item1).ToArray();

            var firstX = lines.Min(l => l.Item1);
            var lastX = lines.Max(l => l.Item1);

            // Add fillament border
            if (firstX > minX)
                additionalLines.Add(new(minX, y, firstX - minX));
            if (lastX < maxX)
                additionalLines.Add(new(lastX + 1, y, maxX - lastX));
            
            var horizontalLines2 = horizontalLinesPerY.ContainsKey(y)
                ? horizontalLinesPerY[y]
                : null;
            for (var i = 0; i < lines.Length; i++) {
                var line1 = lines[i];
                var horizontalLine = horizontalLines2?.Any(l => l.Item1 == line1.Item1);
                if (horizontalLine != true)
                {
                    var line2 = lines.Length > i + 1 ? lines[i+1] : null;
                    if (line2 != null && line2.Item1 > line1.Item1 + 1)
                    {
                        additionalLines.Add(new(line1.Item1 + 1, y, line2.Item1 - line1.Item1 - 1));
                    }
                }
            }
        }

        // Remove if nothing above & not a line
        var queue = new Queue<Tuple<int,int,int>>();
        queue.Enqueue(new(minX - 2, minY - 1, maxX - minX + 4));
        queue.Enqueue(new(minX - 2, maxY + 1, maxX - minX + 4));
        var fillamentPerY = additionalLines.GroupBy(l => l.Item2).ToDictionary(g => g.Key, g => g.ToList());
        
        foreach (var line in additionalLines.Where(l => l.Item1 == minX || l.Item1 + l.Item3 - 1 == maxX))
            queue.Enqueue(line);

        while (queue.Count > 0) {
            var line = queue.Dequeue();
            
            if (line.Item1 > minX - 2) {
                fillamentPerY[line.Item2].Remove(line);
            }
            var begin = line.Item1;
            var end = line.Item1 + line.Item3 - 1;

            // Remove adjacent lines below
            if (line.Item2 < maxY) {
                var linesBelow = fillamentPerY[line.Item2 + 1].Where(l => 
                            (begin >= l.Item1 && begin <= l.Item1 + l.Item3 - 1)
                            || (end >= l.Item1 && end <= l.Item1 + l.Item3 - 1)
                            || (begin < l.Item1 && end > l.Item1 + l.Item3 - 1)
                    ).ToArray();
                foreach (var l in linesBelow) {
                    queue.Enqueue(l);
                }
            }

            // Remove adjacent lines above
            if (line.Item2 > minY) {
                var linesAbove = fillamentPerY[line.Item2 - 1].Where(l => 
                            (begin >= l.Item1 && begin <= l.Item1 + l.Item3 - 1)
                            || (end >= l.Item1 && end <= l.Item1 + l.Item3 - 1)
                            || (begin < l.Item1 && end > l.Item1 + l.Item3 - 1)
                    ).ToArray();
                foreach (var l in linesAbove) {
                    queue.Enqueue(l);
                }
            }
        }

        var lineTotal = horizontalLines.Sum(l => (long)l.Item3) + verticalLines.Sum(l => (long)l.Item3 - 2);
        var fillamentTotal = fillamentPerY.Values.Sum(g => g.Sum(l => (long)l.Item3));
        var total = lineTotal + fillamentTotal;
        Assert.Equal(expectedResult, total);
    }
}