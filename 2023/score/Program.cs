using System.Text.Json;

var input = JsonSerializer.Deserialize<ScoreFile>(File.ReadAllText("input/input.txt"), new JsonSerializerOptions {
    PropertyNameCaseInsensitive = true
});

var participants = input.Members.Values.Order().ToArray();
for (var i = 0; i < participants.Length; i++) {
    var participant = participants[i];
    if (i == 0)
        Console.WriteLine(participant.Name + $" ({participant.Stars} starts)");
    else {
        var loses = participant.LosesFrom(participants[i - 1]);
        Console.WriteLine(participant.Name + $" ({participant.Stars} stars, {loses} times slower than upper participant)");
    }
}

record ScoreFile(Dictionary<string, Participant> Members);
record Participant(string Name, int Stars, Dictionary<string, DayScore> Completion_Day_Level) : IComparable
{
    public int CompareTo(object? obj)
    {
        if (obj is not Participant)
            throw new NotImplementedException();

        var other = (Participant)obj;
        if (Stars == other.Stars)
        {
            return LosesFrom(other);
        }
        return Stars > other.Stars ? -1 : 1;
    }

    public int LosesFrom(Participant other) {
        var result = 0;
        for (var day = 1; day <= 25; day++) {
            for (var part = 1; part <= 2; part++) {
                var finished = HasFinished(day, part);
                var otherFinished = other.HasFinished(day, part);
                if (finished && !otherFinished)
                    result += -1;
                else if (!finished && otherFinished)
                    result += 1;
                else if (finished && otherFinished)
                    result += GetCompletionTs(day, part) < other.GetCompletionTs(day, part) ? -1 : 1;
            }
        }
        return result;
    }

    public bool HasFinished(int day, int part) {
        return Completion_Day_Level.ContainsKey("" + day) && Completion_Day_Level["" + day].ContainsKey("" + part);
    }

    public int GetCompletionTs(int day, int part) {
        return Completion_Day_Level["" + day]["" + part].Get_Star_Ts;
    }
}
class DayScore : Dictionary<string, StarScore> {

}
record StarScore(int Get_Star_Ts);