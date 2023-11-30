using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Group : BaseEntity
{
    public string Name { get; set; }
    public List<Student>? GroupStudents { get; set; }

    public string? Term { get; set; }

    public LessonType LessonType { get; set; } //Offline , Online

    [NotMapped]
    public Guid LessonTypeId { get; set; } 

    [NotMapped]
    public Guid CourceId { get; set; }
    public CourceName CourceName { get; set; }
    public List<Lesson>? Lessons { get; set; }
    public List<UserDaySchedule>? DaySchedules { get; set;}
    public int LessonsCount { get; set; }


    [NotMapped]
    public double AverageAgeStudents { get; set; }

    public Teacher? Teacher { get; set; }


    public Guid TeacherId { get; set; }

}
