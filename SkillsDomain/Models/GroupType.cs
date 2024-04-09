using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class GroupType : BaseEntity
{
    public GroupType? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public string? Name { get; set; }
    public int MinimumStudents { get; set; }
    public int MaximumStudents { get; set; }
}