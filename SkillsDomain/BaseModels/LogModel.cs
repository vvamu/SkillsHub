using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.BaseModels;

public abstract class LogModel<T> : BaseEntity
{
    public T? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public string? Name { get; set; }
    public DateTime? DateRegistration { get; set; }

    public DateTime? DateCreated { get; set; }

    [NotMapped]
    public List<T>? Parents { get; set; }

    [NotMapped]
    public List<T>? Children { get; set; }

    [NotMapped]
    public virtual string? DisplayName { get; }

    public abstract bool Equals(object obj);

}
