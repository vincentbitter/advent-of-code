namespace Lib;

public static class Parser {
    public static string ReadAllText(string fileName) {
        return File.ReadAllText("input/" + fileName);
    }

    public static string[] ReadAllLines(string fileName) {
        return File.ReadAllLines("input/" + fileName);
    }

    public static int[] ReadCsvToInt(string fileName) {
        return ReadAllText(fileName)
            .Split(',')
            .Select(i => int.Parse(i))
            .ToArray();
    }

    public static int[] ReadLinesToInt(string fileName) {
        return ReadAllLines(fileName)
            .Select(i => int.Parse(i))
            .ToArray();
    }
}