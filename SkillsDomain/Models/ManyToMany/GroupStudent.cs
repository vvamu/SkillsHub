using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class GroupStudent : BaseEntity
{
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; }
    public DateTime DateAdd { get; set; } = DateTime.Now;

    public override bool Equals(object obj)
    {
        return false;
    }
}
