using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class LessonTypeTeacher : BaseEntity
{

    public Guid LessonTypeId { get; set; }
    public LessonType LessonType { get; set; }

    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    public DateTime DateAdd { get; set; }

}
