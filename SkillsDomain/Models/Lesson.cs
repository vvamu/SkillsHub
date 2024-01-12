using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models.NotInUse;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Lesson : BaseEntity
{
    public string? Topic { get; set; }
    //[Url]
    public string? LinkToWebinar { get; set; }

    public Teacher? Teacher { get; set; }
    public Guid? TeacherId { get; set; } 
    public List<LessonStudent>? ArrivedStudents { get; set; }
    public Group? Group { get; set; }
    public Guid? GroupId { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    [NotMapped]
    public int LessonTime
    {
        get
        {
            int resultMinutes = 0;
            return this.EndTime.Minute - this.StartTime.Minute;
        }
    }

    public LessonType? LessonType { get; set; } //Group, Individual
    public Guid? LessonTypeId { get; set; } //Group, Individual
    public string? Location { get; set; } //Online Offline


    public LessonActivityType? LessonActivityType { get; set; } //Отработка Пробное убрать

    public LessonTask? LessonTask { get; set; }
    public Guid? LessonTaskId { get; set; }
    public ApplicationUser? Creator { get; set; }
    public bool IsСompleted { get; set; } = false;


    //When lesson "Completed" - From LessonType
    public decimal TeacherPrice { get; set; }
    public decimal StudentPrice {  get; set; }

    public bool IsVerified { get; set; }

}


