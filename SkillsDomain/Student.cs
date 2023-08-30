namespace SkillsDomain;

public class Student : BaseHumanInfo
{
    public string? ParentPhone { get; set; }
    public string EnglishLevel { get; set; }
    public string IsRecorded { get; set; }
    public int CountPayedLessons { get; set; }
    public int VisitedLessons { get; set; }

}