using Lib;

namespace day01;

public class Test
{
    [Theory]
    [InlineData("sample.txt", 142)]
    [InlineData("input.txt", 55477)]
    public void PartA(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);

        var output = 0;
        foreach (var line in input) {
            var num = "" + line.First(i => i >= 48 && i <= 57);
            num += line.Last(i => i >= 48 && i <= 57);
            output += int.Parse(num);
        }
        Assert.Equal(expectedResult, output);
    }

    [Theory]
    [InlineData("sample2.txt", 281)]
    [InlineData("sample3.txt", 28)]
    [InlineData("input.txt", 54431)]
    public void PartB(string fileName, int expectedResult)
    {
        var input = Parser.ReadAllLines(fileName);

        var output = 0;
        foreach (var line in input) {
            var line2 = "";
            var rest = line;
            while(rest != "") {
                if (rest.StartsWith("one")) {
                    line2 += "1";
                    rest = rest.Substring(3);
                }
                else if (rest.StartsWith("two")) {
                    line2 += "2";
                    rest = rest.Substring(3);
                }
                else if (rest.StartsWith("three")) {
                    line2 += "3";
                    rest = rest.Substring(5);
                }
                else if (rest.StartsWith("four")) {
                    line2 += "4";
                    rest = rest.Substring(4);
                }
                else if (rest.StartsWith("five")) {
                    line2 += "5";
                    rest = rest.Substring(4);
                }
                else if (rest.StartsWith("six")) {
                    line2 += "6";
                    rest = rest.Substring(3);
                }
                else if (rest.StartsWith("seven")) {
                    line2 += "7";
                    rest = rest.Substring(5);
                }
                else if (rest.StartsWith("eight")) {
                    line2 += "8";
                    rest = rest.Substring(5);
                }
                else if (rest.StartsWith("nine")) {
                    line2 += "9";
                    rest = rest.Substring(4);
                } else {
                    line2 += rest[0];
                    rest = rest.Substring(1);
                }   
            }

            var line3 = "";
            rest = line;
            while(rest != "") {
                if (rest.EndsWith("one")) {
                    line3 += "1";
                    rest = rest.Substring(0, rest.Length - 3);
                }
                else if (rest.EndsWith("two")) {
                    line3 += "2";
                    rest = rest.Substring(0, rest.Length - 3);
                }
                else if (rest.EndsWith("three")) {
                    line3 += "3";
                    rest = rest.Substring(0, rest.Length - 5);
                }
                else if (rest.EndsWith("four")) {
                    line3 += "4";
                    rest = rest.Substring(0, rest.Length - 4);
                }
                else if (rest.EndsWith("five")) {
                    line3 += "5";
                    rest = rest.Substring(0, rest.Length - 4);
                }
                else if (rest.EndsWith("six")) {
                    line3 += "6";
                    rest = rest.Substring(0, rest.Length - 3);
                }
                else if (rest.EndsWith("seven")) {
                    line3 += "7";
                    rest = rest.Substring(0, rest.Length - 5);
                }
                else if (rest.EndsWith("eight")) {
                    line3 += "8";
                    rest = rest.Substring(0, rest.Length - 5);
                }
                else if (rest.EndsWith("nine")) {
                    line3 += "9";
                    rest = rest.Substring(0, rest.Length - 4);
                } else {
                    line3 += rest.Last();
                    rest = rest.Substring(0, rest.Length - 1);
                }   
            }

            var num = "" + line2.First(i => i >= 48 && i <= 57);
            num += line3.First(i => i >= 48 && i <= 57);
            output += int.Parse(num);
        }
        Assert.Equal(expectedResult, output);
    }
}