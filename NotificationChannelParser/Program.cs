using System.Text.RegularExpressions;

class NotificationChannelParser
{
    private static readonly HashSet<string> validChannels = ["BE", "FE", "QA", "Urgent"];

    public static string ParseNotificationTitle(string title)
    {
        var channels = ExtractChannels(title);
        return FormatOutput(channels);
    }

    private static IEnumerable<string> ExtractChannels(string title)
    {
        var tagPattern = @"(?:\[([^\]]+)\])|(\b(?:BE|FE|QA|Urgent)\b)";
        var matches = Regex.Matches(title, tagPattern, RegexOptions.IgnoreCase);

        return matches
            .SelectMany(m => m.Groups.Cast<Group>().Skip(1))
            .Where(g => g.Success && !string.IsNullOrEmpty(g.Value))
            .Select(g => g.Value)
            .Distinct()
            .Where(tag => validChannels.Contains(tag, StringComparer.OrdinalIgnoreCase));
    }

    private static string FormatOutput(IEnumerable<string> channels)
    {
        var channelList = string.Join(", ", channels);
        return $"Receive channels: {channelList}";
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Enter a notification title:");
        string? input = Console.ReadLine();

        if (String.IsNullOrWhiteSpace(input)) {
            Console.WriteLine("Title is required");
            return;
        }

        string result = ParseNotificationTitle(input);
        Console.WriteLine(result);
    }
}
