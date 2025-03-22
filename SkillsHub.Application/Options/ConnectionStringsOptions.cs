namespace SkillsHub.Application.Options;

public class ConnectionStringsOptions
{
    public const string OptionName = "ConnectionStrings";
    public string DefaultConnection { get; set; }
    public string SqliteConnection { get; set; }
}
