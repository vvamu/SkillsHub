namespace SkillsHub.Domain.BaseModels;

public class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; }

}
