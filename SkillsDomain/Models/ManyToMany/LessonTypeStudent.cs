using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class LessonTypeStudent : LogModel<LessonTypeStudent>
{
    public Student Student { get; set; }
    public Guid? StudentId { get; set; }
    public LessonType LessonType { get; set; }
    public Guid LessonTypeId { get; set; }

    //[NotMapped]
    //public string? StatusChanged { get => Parent?. }

    public override bool Equals(object obj)
    {
        var other = obj as LessonTypeStudent;
        return (StudentId == other.StudentId && LessonTypeId == other.LessonTypeId);
    }
}