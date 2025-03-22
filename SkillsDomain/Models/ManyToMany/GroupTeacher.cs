using SkillsHub.Domain.BaseModels;
using System.ComponentModel;

namespace SkillsHub.Domain.Models;

public class GroupTeacher : BaseEntity
{
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    public Teacher Teacher { get; set; }
    public Guid TeacherId { get; set; }

    [DefaultValue("CONVERT(datetime, GETDATE())")]
    public DateTime DateAdd { get; set; } = DateTime.Now;

    public override bool Equals(object obj)
    {
        var other = obj as GroupTeacher;
        if (other == null) return false;
        return true;
    }
}