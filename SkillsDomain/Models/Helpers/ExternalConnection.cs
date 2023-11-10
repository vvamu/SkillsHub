using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class ExternalConnection : BaseEntity
{
    public string Name { get; set; }
    public string UserProfile { get; set; }
}
