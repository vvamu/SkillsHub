using SkillsHub.Domain.BaseModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Group : BaseEntity
{
    public string Name { get; set; }
    public List<GroupStudent>? GroupStudents { get; set; }
    public List<GroupTeacher>? GroupTeachers { get; set; }


    public PaymentCategory? PaymentCategory { get; set; }
    [ForeignKey("PaymentCategory")]
    public Nullable<Guid> PaymentCategoryId { get; set; }

    public Course Course { get; set; } //Offline , Online
    public Guid CourseId { get; set; }

    public GroupType? GroupType { get; set; }
    public Nullable<Guid> GroupTypeId { get; set; }

    public LocationType? LocationType { get; set; }
    public Nullable<Guid> LocationTypeId { get; set; }

    public int LessonTimeInMinutes { get; set; } // --------

    public bool IsUnlimitedLessonsCount { get; set; }
    public bool IsPermanentStaffGroup { get; set; }
    public bool IsLateDateStart { get; set; }

    [DefaultValue("A1")]
    public string? EnglishLevel { get; set; } = "A1";


    public List<Lesson>? Lessons { get; set; }
    public List<WorkingDay>? DaySchedules { get; set;}
    public int LessonsCount { get; set; }//not mapped
    public int MonthCount { get; set; }//not mapped

    [DefaultValue("CONVERT(date, GETDATE())")]
    public DateTime DateStart { get; set; }
    public bool IsVerified { get; set; } = true;

    #region Helpers

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


    /*

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
    }*/

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
    #endregion

}
