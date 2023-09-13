namespace SkillsHub.Domain.BaseModels;

public class BaseEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }

}
