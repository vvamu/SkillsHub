using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class LessonTypeStudent : BaseEntity
{
    public Student Student { get; set; }
    public Guid? StudentId { get; set; }
    public LessonType LessonType { get; set; }
    public Guid LessonTypeId { get; set; }
    public DateTime DateAdd { get; set; }

}