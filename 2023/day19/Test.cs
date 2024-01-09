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
            .Select(l => l[1..^1])
            .Select(l => l.Split(',')
                .Select(v => int.Parse(v[2..]))
                .ToArray()
            )
            .Select(a => new Part(a[0], a[1], a[2], a[3]))
            .ToArray();

        var inFlow = workflows["in"];
        var accepted = new List<Part>();
        foreach (var part in parts)
        {
            var tests = inFlow;
            var finished = false;
            while (!finished)
            {
                foreach (var test in tests)
                {
                    var result = test(part);
                    if (test != null)
                    {
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
        var unfinished = new Queue<PartRange>();
        unfinished.Enqueue(
            new (new (1, 4000),new (1, 4000),new (1, 4000),new (1, 4000), "in")
        );

        while (unfinished.Any())
        {
            var partRange = unfinished.Dequeue();
            var workflow = workflows[partRange.NextWorkflow!];
            var partRangeLeft = partRange;
            foreach (var test in workflow)
            {
                var newRanges = test(partRangeLeft).ToArray();
                if (newRanges.Length == 1)
                {
                    if (newRanges[0].NextWorkflow == "A")
                    {
                        accepted.Add(newRanges[0]);
                        break;
                    }
                    else if (newRanges[0].NextWorkflow == "R")
                    {
                        break;
                    }
                    else if (newRanges[0].NextWorkflow == null)
                    {
                        partRangeLeft = newRanges[0];
                    }
                    else if (newRanges[0].NextWorkflow != null)
                    {
                        unfinished.Enqueue(newRanges[0]);
                        break;
                    }
                }
                else if (newRanges.Length == 2)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        if (newRanges[i].NextWorkflow == null)
                            partRangeLeft = newRanges[i];
                        else if (newRanges[i].NextWorkflow == "A")
                            accepted.Add(newRanges[i]);
                        else if (newRanges[i].NextWorkflow != "R")
                            unfinished.Enqueue(newRanges[i]);
                    }
                }
            }
        }

        var total = accepted.Sum(p => p.Score());
        Assert.Equal(expectedResult, total);
    }

    private IEnumerable<Func<Part, string?>> ConvertToFunctions(string workflow)
    {
        workflow = workflow.Substring(1, workflow.Length - 2);
        var tests = workflow.Split(',');
        foreach (var test in tests)
        {
            if (test.Contains(':'))
                yield return ConvertToFunction(test);
            else yield return (Part input) => test;
        }
    }

    private Func<Part, string?> ConvertToFunction(string test)
    {
        var colonIndex = test.IndexOf(':');
        var value = int.Parse(test.Substring(2, colonIndex - 2));
        var target = test.Substring(colonIndex + 1);
        var isLt = test[1] == '<';
        switch (test[0])
        {
            case 'x':
                if (isLt)
                    return (Part part) => part.X < value ? target : null;
                return (Part part) => part.X > value ? target : null;
            case 'm':
                if (isLt)
                    return (Part part) => part.M < value ? target : null;
                return (Part part) => part.M > value ? target : null;
            case 'a':
                if (isLt)
                    return (Part part) => part.A < value ? target : null;
                return (Part part) => part.A > value ? target : null;
            case 's':
                if (isLt)
                    return (Part part) => part.S < value ? target : null;
                return (Part part) => part.S > value ? target : null;
            default: throw new NotImplementedException();
        }
    }

    private static IEnumerable<Func<PartRange, IEnumerable<PartRange>>> ConvertToRangeFunctions(string workflow)
    {
        workflow = workflow[1..^1];
        var tests = workflow.Split(',');
        foreach (var test in tests)
        {
            if (test.Contains(':'))
                yield return ConvertToRangeFunction(test);
            else
                yield return (PartRange input) => new List<PartRange> { input with { NextWorkflow = test } };
        }
    }

    private static Func<PartRange, IEnumerable<PartRange>> ConvertToRangeFunction(string test)
    {
        var colonIndex = test.IndexOf(':');
        var value = int.Parse(test.Substring(2, colonIndex - 2));
        var target = test.Substring(colonIndex + 1);
        var isLt = test[1] == '<';
        switch (test[0])
        {
            case 'x':
                if (isLt)
                    return (PartRange range) => range.SplitX(value, target, null);
                return (PartRange range) => range.SplitX(value + 1, null, target);
            case 'm':
                if (isLt)
                    return (PartRange range) => range.SplitM(value, target, null);
                return (PartRange range) => range.SplitM(value + 1, null, target);
            case 'a':
                if (isLt)
                    return (PartRange range) => range.SplitA(value, target, null);
                return (PartRange range) => range.SplitA(value + 1, null, target);
            case 's':
                if (isLt)
                    return (PartRange range) => range.SplitS(value, target, null);
                return (PartRange range) => range.SplitS(value + 1, null, target);
            default: throw new NotImplementedException();
        }
    }
}