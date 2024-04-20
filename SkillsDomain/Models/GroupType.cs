using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class GroupType : BaseEntity
{
    public GroupType? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public string? Name { get; set; }
    public int MinimumStudents { get; set; }
    public int MaximumStudents { get; set; }

    [NotMapped]
    public string? DisplayName
    {
        get
        {
            var res = Name;
            if (MinimumStudents == MaximumStudents) res += " (" + MinimumStudents + " students)";
            else res += " (" + MinimumStudents + " - " + MaximumStudents + " students)";
            return res;
        }
    }
}