using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Teacher : BaseEntity
{
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    [NotMapped]
    public decimal CalculatedSalary { get; set; }
    public List<Group>? Groups { get; set; }
    public string? WorkingDays { get; set; }

    public List<CourseNameTeacher>? PossibleCources { get; set; }


}