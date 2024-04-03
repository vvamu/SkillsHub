using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models.NotInUse;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Lesson : BaseEntity
{
    public string? Topic { get; set; }
    public Teacher? Teacher { get; set; }
    public Guid? TeacherId { get; set; } 
    public List<LessonStudent>? ArrivedStudents { get; set; }
    public Group? Group { get; set; }
    public Guid? GroupId { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    
    /*

    public LessonType? LessonType { get; set; } //Group, Individual
    public Guid? LessonTypeId { get; set; } 
    */
    public string? Location { get; set; } //Online Offline


    //public LessonActivityType? LessonActivityType { get; set; } //Отработка Пробное убрать
    public LessonTask? LessonTask { get; set; }
    public Guid? LessonTaskId { get; set; }
    //public ApplicationUser? Creator { get; set; }
    //public Guid CreatorId { get; set; }

    public string? Comment { get; set; } = "";
    public string? AdditionalMaterials { get; set; } = "";

    [NotMapped]
    public bool IsСompleted { get; set; } = false;
    public DateTime DateCompletedByTeacher { get; set; }

    //public bool IsVerified { get; set; }
    #region Helpers

    [NotMapped]
    public int Duration
    {
        get
        {
            int resultMinutes = 0;
            return (int)(EndTime - StartTime).TotalMinutes;
        }
    }

    #endregion

}


