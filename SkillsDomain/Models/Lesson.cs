using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class Lesson : BaseEntity
{
    public Teacher? Teacher { get; set; }
    public Guid? TeacherId { get; set; }
    public List<LessonStudent>? ArrivedStudents { get; set; }
    public Group? Group { get; set; }
    public Guid? GroupId { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public LessonType? LessonType { get; set; } //Group, Individual
    public Guid? LessonTypeId { get; set; } //Group, Individual
    public string? Location { get; set; } //Online Offline


    public LessonActivityType? LessonActivityType { get; set; } //Отработка Пробное убрать

    public LessonTask? LessonTask { get; set; }
    public Guid? LessonTaskId { get; set; }
    public string? LinkToWebinar { get; set; }
    public ApplicationUser? Creator { get; set; }
    public bool IsСompleted { get; set; } = false;


    //When lesson "Completed" - From LessonType
    public decimal TeacherPrice { get; set; }
    public decimal StudentPrice {  get; set; }

}


