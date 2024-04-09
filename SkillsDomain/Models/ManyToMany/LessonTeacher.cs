using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Persistence;

public class LessonTeacher :  BaseEntity
{
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; }
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    public DateTime DateAdd { get; set; }
}