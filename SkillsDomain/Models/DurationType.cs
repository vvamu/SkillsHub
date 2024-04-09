using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class DurationType : BaseEntity
{
    public DurationType? Parent { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }
}