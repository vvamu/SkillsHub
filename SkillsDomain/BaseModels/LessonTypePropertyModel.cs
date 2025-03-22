using SkillsHub.Domain.Models;

namespace SkillsHub.Domain.BaseModels;

public abstract class LessonTypePropertyModel<T> : LogModel<T>
{
    public List<LessonType>? LessonTypes { get; set; }

}
