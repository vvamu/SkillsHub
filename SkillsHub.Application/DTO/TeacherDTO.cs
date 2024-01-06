using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.DTO;

public class TeacherDTO : ApplicationUser
{
    public Guid UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public decimal CalculatedSalary { get; set; }
    public List<CourseName>? PossibleCources { get; set; }
    public List<Lesson>? CurrentLessons { get; set; }
    public List<Lesson>? TotalLessons { get; set; }


}