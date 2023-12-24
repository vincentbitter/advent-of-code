using Lib;
using Microsoft.Z3;

namespace day24;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 7, 27, 2)]
    [InlineData("input.txt", 200000000000000, 400000000000000, 17776)]
    public void PartA(string fileName, long min, long max, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var lines = input.Select(line => line.Split(" @ ")).Select(p => new {
                Item1 = p[0].Split(", ").Select(long.Parse).ToArray(),
                Item2 = p[1].Split(", ").Select(int.Parse).ToArray()
            })
            .Select(x => new Heartstone(x.Item1[0],x.Item1[1],x.Item1[2], x.Item2[0], x.Item2[1], x.Item2[2]))
            .ToArray();

        var total = 0;
        for (var i = 0; i < lines.Length - 1; i++) {
            for (var j = i + 1; j < lines.Length; j++) {
                var x = lines[i].Intersects(lines[j], false);
                if (x != null && x.X >= min && x.X <= max && x.Y >= min && x.Y <= max)
                    total++;
            }
        }
        Assert.Equal(expectedResult, total);
    }

    [Theory]
    [InlineData("sample.txt", 47)]
    [InlineData("input.txt", 948978092202212)]
    public void PartB(string fileName, long expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);
        var lines = input.Select(line => line.Split(" @ ")).Select(p => new {
                Item1 = p[0].Split(", ").Select(long.Parse).ToArray(),
                Item2 = p[1].Split(", ").Select(int.Parse).ToArray()
            })
            .Select(x => new Heartstone(x.Item1[0],x.Item1[1],x.Item1[2], x.Item2[0], x.Item2[1], x.Item2[2]))
            .ToArray();

        using var ctx = new Context();

        // Define begin point for new line
        var x = ctx.MkIntConst("x");
        var y = ctx.MkIntConst("y");
        var z = ctx.MkIntConst("z");
        var vx = ctx.MkIntConst("vx");
        var vy = ctx.MkIntConst("vy");
        var vz = ctx.MkIntConst("vz");
        
        var solver = ctx.MkSolver();
        
        for (var i = 0; i < lines.Length; i++) {
            // Time for hit can change per line
            var t = ctx.MkIntConst("t" + i);

            // Add existing line
            var hx = ctx.MkInt(lines[i].X);
            var hy = ctx.MkInt(lines[i].Y);
            var hz = ctx.MkInt(lines[i].Z);
            
            var hvx = ctx.MkInt(lines[i].VX);
            var hvy = ctx.MkInt(lines[i].VY);
            var hvz = ctx.MkInt(lines[i].VZ);

            // Determine position for existing line after time
            var x2 = ctx.MkAdd(x, ctx.MkMul(t, vx));
            var y2 = ctx.MkAdd(y, ctx.MkMul(t, vy));
            var z2 = ctx.MkAdd(z, ctx.MkMul(t, vz));

            // Determine position for new line after time
            var hx2 = ctx.MkAdd(hx, ctx.MkMul(t, hvx));
            var hy2 = ctx.MkAdd(hy, ctx.MkMul(t, hvy));
            var hz2 = ctx.MkAdd(hz, ctx.MkMul(t, hvz));

            // Find moment in time the lines hit
            solver.Add(t >= 0);
            solver.Add(ctx.MkEq(x2, hx2));
            solver.Add(ctx.MkEq(y2, hy2));
            solver.Add(ctx.MkEq(z2, hz2));
        }

        solver.Check();
        var model = solver.Model;

        // Find starting point for the new line
        var ix = model.Eval(x);
        var iy = model.Eval(y);
        var iz = model.Eval(z);

        var result = long.Parse(ix.ToString()) + long.Parse(iy.ToString()) + long.Parse(iz.ToString());
        Assert.Equal(expectedResult, result);
    }
}

public record Heartstone(long X, long Y, long Z, int VX, int VY, int VZ) {
    public Line2D GetLine2D(long min, long max) {
        var timeHorizontal = VX < 0 ? (min - X) / (double)VX : (max - X) / (double)VX;
        var yWhenAtHorizontalBorder = Y + (VY * timeHorizontal);
        if (yWhenAtHorizontalBorder < min || yWhenAtHorizontalBorder > max) {
            var timeVertical = VY < 0 ? (min - Y) / (double)VY : (max - Y) / (double)VY;
            var xWhenAtVerticalBorder = X + (VX * timeVertical);

            return VY < 0 
                ? new Line2D(new Point2D(xWhenAtVerticalBorder, min), new Point2D(X, Y))
                : new Line2D(new Point2D(X, Y), new Point2D((long)xWhenAtVerticalBorder, min));
        }

        return VX < 0 
            ? new Line2D(new Point2D(min, yWhenAtHorizontalBorder), new Point2D(X, Y))
            : new Line2D(new Point2D(X, Y), new Point2D(max, (long)yWhenAtHorizontalBorder));
    }

    public Point2D? Intersects(Heartstone other, bool includeZ){
        var m = VY / (double)VX;
        var a = -m;
        var b = 1;
        var c = Y - m * X;
        var m2 = other.VY / (double)other.VX;
        var a2 = -m2;
        var b2 = 1;
        var c2 = other.Y - m2 * other.X;

        double delta = a * b2 - a2 * b;

        if (delta == 0)
            return null;

        double x = (b2 * c - b * c2) / delta;
        double y = (a * c2 - a2 * c) / delta;
        var time = Math.Abs(((X - x) / VX));

        if (includeZ) {
            double z1 = Z + (time * VZ);
            double z2 = other.Z + (time * other.VZ);

            if (z1 != z2)
                return null;
        }

        var point = new Point2D(x, y);
        if (InLine(point) && other.InLine(point))
            return point;
        return null;
    }

    public bool InLine(Point2D point) {
        if ((VX < 0 && point.X > X) || (VX > 0 && point.X < X))
            return false;
        if ((VY < 0 && point.Y > Y) || (VY > 0 && point.Y < Y))
            return false;
        return true;
    }
}

public record Line2D(Point2D From, Point2D To);

public record Point2D(double X, double Y);