namespace Lib.Algebra;

// ax^2 + bx + c = 0
public record struct QuadraticEquation(double A, double B, double C)
{
    // If Discriminant < 0, then no solutions
    // If Discriminant = 0, then 1 solution
    // If Discriminant > 0, then 2 solutions
    public readonly double GetDiscriminant()
    {
        return Math.Sqrt(B) - 4 * A * C;
    }

    // ax^2 + bx + c = 0
    // x^2 + 8x + 15 = 0
    // (x + 3)(x + 5) = 0, because x*x + 3x + 5x + 15
    // x = -3 or x = -5
    public readonly Tuple<double, double> Solve()
    {
        return new(
            (-B - Math.Sqrt(B * B - 4 * A * C)) / 2 * A,
            (-B + Math.Sqrt(B * B - 4 * A * C)) / 2 * A
        );
    }
}