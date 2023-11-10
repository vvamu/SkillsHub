using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class Teacher : BaseEntity
{
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public decimal CalculatedSalary { get; set; }
    public List<Lesson>? Lessons { get; set; }
}