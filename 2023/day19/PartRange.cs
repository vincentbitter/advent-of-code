using Lib.Misc;

namespace day19;

public record PartRange(LongRange X, LongRange M, LongRange A, LongRange S, string? NextWorkflow)
{
    public IEnumerable<PartRange> SplitX(int value, string? workflowIfLower, string? workflowIfEqualOrHigher)
    {
        if (X.To < value)
            yield return new PartRange(X, M, A, S, workflowIfLower);
        else if (X.From >= value)
            yield return new PartRange(X, M, A, S, workflowIfEqualOrHigher);
        else
        {
            var p = X.Split(value);
            yield return new PartRange(p.First(), M, A, S, workflowIfLower);
            yield return new PartRange(p.Last(), M, A, S, workflowIfEqualOrHigher);
        }
    }
    public IEnumerable<PartRange> SplitM(int value, string? workflowIfLower, string? workflowIfEqualOrHigher)
    {
        if (M.To < value)
            yield return new PartRange(X, M, A, S, workflowIfLower);
        else if (M.From >= value)
            yield return new PartRange(X, M, A, S, workflowIfEqualOrHigher);
        else
        {
            var p = M.Split(value);
            yield return new PartRange(X, p.First(), A, S, workflowIfLower);
            yield return new PartRange(X, p.Last(), A, S, workflowIfEqualOrHigher);
        }
    }
    public IEnumerable<PartRange> SplitA(int value, string? workflowIfLower, string? workflowIfEqualOrHigher)
    {
        if (A.To < value)
            yield return new PartRange(X, M, A, S, workflowIfLower);
        else if (A.From >= value)
            yield return new PartRange(X, M, A, S, workflowIfEqualOrHigher);
        else
        {
            var p = A.Split(value);
            yield return new PartRange(X, M, p.First(), S, workflowIfLower);
            yield return new PartRange(X, M, p.Last(), S, workflowIfEqualOrHigher);
        }
    }
    public IEnumerable<PartRange> SplitS(int value, string? workflowIfLower, string? workflowIfEqualOrHigher)
    {
        if (S.To < value)
            yield return new PartRange(X, M, A, S, workflowIfLower);
        else if (S.From >= value)
            yield return new PartRange(X, M, A, S, workflowIfEqualOrHigher);
        else
        {
            var p = S.Split(value);
            yield return new PartRange(X, M, A, p.First(), workflowIfLower);
            yield return new PartRange(X, M, A, p.Last(), workflowIfEqualOrHigher);
        }
    }

    public long Score()
    {
        var combinations = X.Size() * M.Size() * A.Size() * S.Size();
        return combinations;
    }
}