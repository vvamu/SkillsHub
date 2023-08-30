namespace SkillsDomain;

public class Teacher : BaseHumanInfo
{
    public bool IsAdmin { get; set; }
    public bool CalculatedSalary { get; set; }

    public List<Lesson>? CurrentLessons { get; set; }
    public List<Lesson>? TotalLessons { get; set; }


}