using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Domain.DTO;

public class TeacherDTO : ApplicationUser
{
    public bool IsAdmin { get; set; }
    public bool CalculatedSalary { get; set; }

    public List<Lesson>? CurrentLessons { get; set; }
    public List<Lesson>? TotalLessons { get; set; }


}