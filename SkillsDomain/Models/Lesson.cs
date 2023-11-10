using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class Lesson : BaseEntity
{
    public List<Teacher> Teachers { get; set; }
    public List<LessonStudent>? ArrivedStudents { get; set; }
    public Group? Group { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string LessonType { get; set; } //Group, Individual
    public string? Location { get; set; } //Online Offline


    public LessonActivityType? LessonActivityType { get; set; } //Отработка Пробное

    public LessonTask? LessonTask { get; set; }
    public string LinkToWebinar { get; set; }
    public ApplicationUser Creator { get; set; }
    public bool IsСompleted { get; set; }



}


