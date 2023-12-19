using Lib;

namespace day19;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 19114)]
    [InlineData("input.txt", 323625)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName).ToList();
        var split = input.IndexOf("");
        var workflows = input.Take(split).ToDictionary(
            w => w.Substring(0, w.IndexOf("{")),
            w => ConvertToFunctions(w.Substring(w.IndexOf("{"))).ToArray()
        );
        var parts = input
            .Skip(split + 1)
            .Select(l => l.Substring(1, l.Length - 2))
            .Select(l => l.Split(',')
                .Select(v => int.Parse(v.Substring(2)))
                .ToArray()
            )
            .Select(a => new Part(a[0],a[1],a[2],a[3]))
            .ToArray();

        var inFlow = workflows["in"];
        var accepted = new List<Part>();
        foreach (var part in parts) {
            var tests = inFlow;
            var finished = false;
            while(!finished) {
                foreach (var test in tests) {
                    var result = test(part);
                    if (test != null) {
                        if (result == "A")
                        {
                            accepted.Add(part);
                            finished = true;
                            break;
                        }
                        else if (result == "R")
                        {
                            finished = true;
                            break;
                        }
                        else if (result != null)
                        {
                            tests = workflows[result];
                            break;
                        }
                    }
                }
            }
        }

        var total = accepted.Sum(p => p.X + p.M + p.A + p.S);

        Assert.Equal(expectedResult, total);
    }

    private IEnumerable<Func<Part,string>> ConvertToFunctions(string workflow)
    {
        workflow = workflow.Substring(1, workflow.Length - 2);
        var tests = workflow.Split(',');
        foreach (var test in tests) {
            if (test.Contains(':'))
                yield return ConvertToFunction(test);
            else yield return (Part input) => test;
        }
    }

    private Func<Part, string> ConvertToFunction(string test)
    {
        var value = int.Parse(test.Substring(2, test.IndexOf(':') - 2));
        var target = test.Substring(test.IndexOf(':') + 1);
        switch (test[0]) {
            case 'x':
                if (test.Contains('<'))
                    return (Part part) => part.X < value ? target : null;
                return (Part part) => part.X > value ? target : null;
            case 'm': 
                if (test.Contains('<'))
                    return (Part part) => part.M < value ? target : null;
                return (Part part) => part.M > value ? target : null;
            case 'a': 
                if (test.Contains('<'))
                    return (Part part) => part.A < value ? target : null;
                return (Part part) => part.A > value ? target : null;
            case 's': 
                if (test.Contains('<'))
                    return (Part part) => part.S < value ? target : null;
                return (Part part) => part.S > value ? target : null;
            default: throw new NotImplementedException();
        }
    }

    private IEnumerable<Func<PartRange,IEnumerable<PartRange>>> ConvertToRangeFunctions(string workflow)
    {
        workflow = workflow.Substring(1, workflow.Length - 2);
        var tests = workflow.Split(',');
        foreach (var test in tests) {
            if (test.Contains(':'))
                yield return ConvertToRangeFunction(test);
            else 
                yield return (PartRange input) => new List<PartRange> { new PartRange(input.X, input.M, input.A, input.S, test) };
        }
    }

    private Func<PartRange,IEnumerable<PartRange>> ConvertToRangeFunction(string test)
    {
        var value = int.Parse(test.Substring(2, test.IndexOf(':') - 2));
        var target = test.Substring(test.IndexOf(':') + 1);
        switch (test[0]) {
            case 'x':
                if (test.Contains('<'))
                    return (PartRange range) => range.SplitX(value, target, null);
                return (PartRange range) => range.SplitX(value + 1, null, target);
            case 'm': 
                if (test.Contains('<'))
                    return (PartRange range) => range.SplitM(value, target, null);
                return (PartRange range) => range.SplitM(value + 1, null, target);
            case 'a': 
                if (test.Contains('<'))
                    return (PartRange range) => range.SplitA(value, target, null);
                return (PartRange range) => range.SplitA(value + 1, null, target);
            case 's': 
                if (test.Contains('<'))
                    return (PartRange range) => range.SplitS(value, target, null);
                return (PartRange range) => range.SplitS(value + 1, null, target);
            default: throw new NotImplementedException();
        }
    }

    private record Part(int X, int M, int A, int S);

    [Theory]
    [InlineData("sample.txt", 167409079868000)]
    [InlineData("input.txt", 127447746739409)]
    public void PartB(string fileName, long expectedResult)
    {
        var input = Parser.ReadAllLines(fileName).ToList();
        var split = input.IndexOf("");
        var workflows = input.Take(split).ToDictionary(
            w => w.Substring(0, w.IndexOf("{")),
            w => ConvertToRangeFunctions(w.Substring(w.IndexOf("{"))).ToArray()
        );

        var accepted = new List<PartRange>();
        var unfinished = new List<PartRange> {
            new PartRange(new Range(1, 4000),new Range(1, 4000),new Range(1, 4000),new Range(1, 4000), "in")
        };

        while (unfinished.Any()) {
            var newUnfinished = new List<PartRange>();
            foreach (var partRange in unfinished) {
                var workflow = workflows[partRange.NextWorkflow];
                var partRangeLeft = partRange;
                foreach (var test in workflow) {
                    var newRanges = test(partRangeLeft).ToArray();
                    if (newRanges.Length == 1 && newRanges[0].NextWorkflow == "A") {
                        accepted.Add(newRanges[0]);
                        break;
                    }
                    else if (newRanges.Length == 1 && newRanges[0].NextWorkflow == "R") {
                        break;
                    }
                    else if (newRanges.Length == 1 && newRanges[0].NextWorkflow == null) {
                        partRangeLeft = newRanges[0];
                    }
                    else if (newRanges.Length == 1 && newRanges[0].NextWorkflow != null) {
                        newUnfinished.Add(newRanges[0]);
                        break;
                    }
                    else if (newRanges.Length == 2) {
                        if (newRanges[0].NextWorkflow == null) {
                            partRangeLeft = newRanges[0];
                        } else if (newRanges[0].NextWorkflow == "A") {
                            accepted.Add(newRanges[0]);
                        } else if (newRanges[0].NextWorkflow != "R") {
                            newUnfinished.Add(newRanges[0]);
                        }

                        if (newRanges[1].NextWorkflow == null) {
                            partRangeLeft = newRanges[1];
                        } else if (newRanges[1].NextWorkflow == "A") {
                            accepted.Add(newRanges[1]);
                        } else if (newRanges[1].NextWorkflow != "R") {
                            newUnfinished.Add(newRanges[1]);
                        }
                    }
                }
            }
            unfinished = newUnfinished;
        }

        var total = accepted.Sum(p => p.Score());
        Assert.Equal(expectedResult, total);
    }
}

public record PartRange(Range X, Range M, Range A, Range S, string NextWorkflow)
{
    public IEnumerable<PartRange> SplitX(int value, string workflowIfLower, string workflowIfEqualOrHigher)
    {
        if (X.To < value)
            yield return new PartRange(X, M, A, S, workflowIfLower);
        else if (X.From >= value)
            yield return new PartRange(X, M, A, S, workflowIfEqualOrHigher);
        else {
            var p = X.Split(value);
            yield return new PartRange(p.First(), M, A, S, workflowIfLower);
            yield return new PartRange(p.Last(), M, A, S, workflowIfEqualOrHigher);
        }
    }
    public IEnumerable<PartRange> SplitM(int value, string workflowIfLower, string workflowIfEqualOrHigher)
    {
        if (M.To < value)
            yield return new PartRange(X, M, A, S, workflowIfLower);
        else if (M.From >= value)
            yield return new PartRange(X, M, A, S, workflowIfEqualOrHigher);
        else {
            var p = M.Split(value);
            yield return new PartRange(X, p.First(), A, S, workflowIfLower);
            yield return new PartRange(X, p.Last(), A, S, workflowIfEqualOrHigher);
        }
    }
    public IEnumerable<PartRange> SplitA(int value, string workflowIfLower, string workflowIfEqualOrHigher)
    {
        if (A.To < value)
            yield return new PartRange(X, M, A, S, workflowIfLower);
        else if (A.From >= value)
            yield return new PartRange(X, M, A, S, workflowIfEqualOrHigher);
        else {
            var p = A.Split(value);
            yield return new PartRange(X, M, p.First(), S, workflowIfLower);
            yield return new PartRange(X, M, p.Last(), S, workflowIfEqualOrHigher);
        }
    }
    public IEnumerable<PartRange> SplitS(int value, string workflowIfLower, string workflowIfEqualOrHigher)
    {
        if (S.To < value)
            yield return new PartRange(X, M, A, S, workflowIfLower);
        else if (S.From >= value)
            yield return new PartRange(X, M, A, S, workflowIfEqualOrHigher);
        else {
            var p = S.Split(value);
            yield return new PartRange(X, M, A, p.First(), workflowIfLower);
            yield return new PartRange(X, M, A, p.Last(), workflowIfEqualOrHigher);
        }
    }

    public long Score() {
        var combinations = (long)X.Count() * (long)M.Count() * (long)A.Count() * (long)S.Count();
        return combinations;
    }
}

public record Range(int From, int To)
{
    public IEnumerable<Range> Split(int value) {
        return new List<Range> {
            new Range(From, value - 1),
            new Range(value, To)
        };
    }

    public int Count() {
        return To - From + 1;
    }

    public double Avg() {
        return (To + From) / 2.0;
    }
}