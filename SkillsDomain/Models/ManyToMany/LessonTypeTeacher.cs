using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class LessonTypeTeacher : LogModel<LessonTypeTeacher>
{
    public Guid LessonTypeId { get; set; }
    public LessonType LessonType { get; set; }

    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public override bool Equals(object obj)
    {
        var other = obj as LessonTypeTeacher;
        return (TeacherId == other.TeacherId && LessonTypeId == other.LessonTypeId);
    }
}
