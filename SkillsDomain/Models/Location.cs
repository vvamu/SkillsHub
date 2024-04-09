using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class Location : BaseEntity
{
    public Location? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsOffline { get; set; }
    public string Description { get; set; }
}