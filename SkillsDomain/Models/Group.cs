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

    [NotMapped]
    public int PassedLessonsCount
    {
        get
        {
            if (this.Lessons != null)
               return this.Lessons.Where(x=>x.IsСompleted).ToList().Count();
            return 0;

        }
    }


    public bool IsUnlimitedLessonsCount { get; set; } 

    [NotMapped]
    public int NeededLessonsTimeInMinutes
    {
        get
        {
            if (this.LessonType != null)
                return this.LessonType.LessonTimeInMinutes * this.LessonsCount;
            else
                return 0;
        }
    }

    [NotMapped]
    public int ResultLessonsTimeInMinutes { get
        {
            int resultMinutes = 0;
            if(this.Lessons != null)
            this.Lessons.ForEach(x => resultMinutes += (x.EndTime.Minute - x.StartTime.Minute));
            return resultMinutes;

        }}

    [NotMapped]
    public int PassedLessonsTimeInMinutes
    {
        get
        {
            int resultMinutes = 0;
            if (this.Lessons != null)
                this.Lessons.Where(x=>x.IsСompleted).ToList().ForEach(x => resultMinutes += (x.EndTime.Minute - x.StartTime.Minute));
            return resultMinutes;

        }
    }

    public DateTime DateStart { get; set; }

    public bool IsLateDateStart { get; set; }

    public Teacher? Teacher { get; set; }

    public Guid TeacherId { get; set; }
    public bool IsVerified { get; set; } = true;
}
