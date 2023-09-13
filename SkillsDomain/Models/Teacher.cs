using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class Teacher : ApplicationUser
{
    public bool IsAdmin { get; set; }
    public bool CalculatedSalary { get; set; }

    public List<Lesson>? CurrentLessons { get; set; }
    public List<Lesson>? TotalLessons { get; set; }


}