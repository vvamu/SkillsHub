using SkillsHub.Domain.BaseModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Group : BaseEntity
{
    public string Name { get; set; }
    public string? EnglishLevel { get; set; }
    public PaymentCategory? PaymentCategory { get; set; }
    public Guid? PaymentTypeId { get; set; }

    public LessonType LessonType { get; set; } 
    [ForeignKey(nameof(LessonType))]
    public Guid LessonTypeId { get; set; }

    public List<GroupStudent>? GroupStudents { get; set; }

    public List<Lesson>? Lessons { get; set; }

    public List<WorkingDay>? DaySchedules { get; set;}

    public DurationType? DurationType { get; set; }
    public int LessonsCount { get; set; }

    public bool IsVerified { get; set; }

    public bool IsUnlimitedLessonsCount { get; set; }

    [NotMapped]
    public int PassedLessonsCount
    {
        get
        {
            if (this.Lessons != null)
                return this.Lessons.Where(x => x.IsСompleted).ToList().Count();
            return 0;

        }
    }


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
    public int ResultLessonsTimeInMinutes
    {
        get
        {
            int resultMinutes = 0;
            if (this.Lessons == null) return resultMinutes;
            foreach (var x in this.Lessons)
            {
                resultMinutes += (int)(x.EndTime - x.StartTime).TotalMinutes;
            }
            if (resultMinutes == 0)
                this.Lessons.ToList().ForEach(x => resultMinutes += (int)(x.EndTime - x.StartTime).TotalMinutes);
            return resultMinutes;

        }
    }

    [NotMapped]
    public int PassedLessonsTimeInMinutes
    {
        get
        {
            int resultMinutes = 0;
            if (this.Lessons != null)
                this.Lessons.Where(x => x.IsСompleted).ToList().ForEach(x => resultMinutes += (int)(x.EndTime - x.StartTime).TotalMinutes);
            return resultMinutes;

        }
    }

    public DateTime DateStart { get; set; }

    public bool IsLateDateStart { get; set; }

    public List<GroupTeacher>? GroupTeachers { get; set; }

    [NotMapped]
    public GroupTeacher? GroupTeacher { get => GroupTeachers.FirstOrDefault(); }
    [NotMapped]

    public Guid TeacherId { get; set; }

    public bool IsPermanentStaffGroup { get; set; }

}
