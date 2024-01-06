using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Group : BaseEntity
{
    public string Name { get; set; }
    public List<GroupStudent>? GroupStudents { get; set; }

    public string? Term { get; set; }

    public LessonType LessonType { get; set; } //Offline , Online
    [ForeignKey(nameof(LessonType))]
    public Guid LessonTypeId { get; set; } 
    
    public CourseName CourseName { get; set; }
    [ForeignKey(nameof(CourseName))]
    public Guid CourseNameId { get; set; }
    public List<Lesson>? Lessons { get; set; }
    public List<WorkingDay>? DaySchedules { get; set;}
    public int LessonsCount { get; set; }

    public DateTime DateStart { get; set; }

    public Teacher? Teacher { get; set; }

    public Guid TeacherId { get; set; }
}
