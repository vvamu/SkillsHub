using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class RequestLesson : BaseEntity
{
    public ApplicationUser? User { get; set; }
    public Lesson? LessonBefore { get; set; }
    public Guid? LessonBeforeId { get; set; }
    public string RequestMessage { get; set; }
    public string? AnswerStatus { get; set; }
    public DateTime NewStart { get; set; }
    public DateTime NewEnd { get; set; }
}
