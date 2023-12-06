using Lib;

namespace dayXX;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 3)]
    [InlineData("input.txt", 10)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadCsvToInt(fileName);
        Assert.Equal(expectedResult, input.Sum());
    }

    [Theory]
    [InlineData("sample.txt", 3)]
    [InlineData("input.txt", 10)]
    public void PartB(string fileName, int expectedResult)
    {

    }
}