using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.BaseModels;

public class BaseEntity
{
    public Guid Id { get; set; }
    [NotMapped]
    public string StringId { get; set; }


    public DateTime DateCreated { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; } = false;

    [NotMapped]
    public string? Status { get => IsDeleted ? "Удален" : "Активен"; }

}
