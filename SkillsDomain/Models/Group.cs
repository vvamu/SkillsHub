using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Group : BaseModels.BaseEntity
{
    public string Name { get; set; }
    public string? EnglishLevel { get; set; }
    public PaymentCategory PaymentCategory { get; set; }
    public Guid? PaymentCategoryId { get; set; }
    public LessonType LessonType { get; set; }
    public Guid LessonTypeId { get; set; }
    public List<GroupTeacher>? GroupTeachers { get; set; }
    public List<GroupStudent>? GroupStudents { get; set; }
    public List<Lesson>? Lessons { get; set; }

    public List<GroupWorkingDay>? DaySchedules { get; set; }

    //[NotMapped] public DurationType? DurationType { get; set; }

    public int ResultLessonsCount { get; set; }

    public bool IsVerified { get; set; }

    public bool IsUnlimitedLessonsCount { get; set; }
    public DateTime DateStart { get; set; }


    public bool IsLateDateStart { get; set; }


    //public DateTime DateLastUpdate { get; set; }

    public bool IsPermanentStaffGroup { get; set; }
    public bool IsCanAddLessons { get; set; } = false;


    #region NotMapped

    [NotMapped]
    public GroupTeacher? GroupTeacher
    {
        get
        {
            if (GroupTeachers == null) return null;
            var res = GroupTeachers?
           .GroupBy(x => x.TeacherId).ToList();
            var res2 = res.Select(g => g.OrderByDescending(x => x.DateAdd).First()).ToList();
            var res3 = res2.Where(x => !x.IsDeleted).First();
            return res3;
        }
    }
    [NotMapped]
    public List<GroupTeacher>? CurrentGroupTeachers
    {
        get
        {
            if (GroupTeachers == null) return new List<GroupTeacher>();

            var res = GroupTeachers?
            .GroupBy(x => x.TeacherId).ToList();
            var res2 = res.Select(g => g.OrderByDescending(x => x.DateAdd).First()).ToList();
            var res3 = res2.Where(x => !x.IsDeleted).ToList();
            return res3;
        }
    }

    [NotMapped]
    public List<GroupStudent>? CurrentGroupStudents
    {
        get
        {
            if (GroupStudents == null) return new List<GroupStudent>();
            var res = GroupStudents?
            .GroupBy(x => x.StudentId).ToList();
            var res2 = res.Select(g => g.OrderByDescending(x => x.DateAdd).First()).ToList();
            var res3 = res2.Where(x => !x.IsDeleted).ToList();
            return res3;
        }
    }



    [NotMapped]
    public IQueryable<IGrouping<DateTime, GroupTeacher>>? LogGroupTeachers { get; set; }
    [NotMapped]

    public IQueryable<IGrouping<DateTime, GroupStudent>>? LogGroupStudents { get; set; }

    [NotMapped]
    public bool IsCreateLessonsAlready { get; set; }
    //public int CountWorkingHours { get; set; }
    [NotMapped]
    public string? PreparedDurationTextRu
    {
        get
        {
            if (IsUnlimitedLessonsCount) return "Не определена";
            if (LessonType == null) return "";
            return LessonType.PreparedDurationTextRu;
        }
    }

    [NotMapped]
    public string? DateStartTextRu
    {
        get
        {
            if (IsLateDateStart || DateStart == DateTime.MinValue) return "Пока не определена";
            return DateStart.ToLongDateString();
        }
    }

    [NotMapped]
    public string? PermanentStaffGroupTextRu
    {
        get
        {
            if (IsPermanentStaffGroup) return "Группа с постоянным составом";
            return "Состав не нормирован";
        }
    }


    #region Lessons
    #endregion
    private int lessonsCount;

    [NotMapped]
    public int LessonsCount
    {
        get
        {
            if (LessonType == null) return 1;
            else return LessonType.PreparedLessonsCount;
        }
        set
        {
            lessonsCount = value;
        }
    }

    public string? NeedToCreateLessonsTextRu
    {
        get
        {
            var needToCreate = "Необходимо объявить: ";
            if (Lessons == null) return needToCreate + "0 занятий";
            return needToCreate + PreparedDurationTextRu;
        }
    }

    #region Passed

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

    public string? PassedLessonsTextRu
    {
        get
        {
            var passed = "Прошло: ";
            if (Lessons == null) return passed + "0 занятий";
            return passed + $"{PassedLessonsCount} занятий ({PassedLessonsTimeInMinutes} минут)";
        }
    }
    #endregion

    #region Created

    [NotMapped]
    public int CreatedLessonsCount
    {
        get
        {
            if (this.Lessons == null) return 0;
            return this.Lessons.Count();

        }
    }

    [NotMapped]
    public int CreatedLessonsTimeInMinutes
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

    public string? CreatedLessonsTextRu
    {
        get
        {
            var created = "Ожидается: ";
            if (Lessons == null) return created + "0 занятий";
            return created + $"{CreatedLessonsCount} занятий ({CreatedLessonsTimeInMinutes} минут)";
        }
    }

    #endregion





    [NotMapped]
    public Guid TeacherId { get; set; }

    [NotMapped]
    public string? TeacherIdString { get; set; }

    public override bool Equals(object obj)
    {
        var other = obj as Group;
        if (other == null) return false;
        if (Name == other.Name && DateStart == other.DateStart && IsLateDateStart && other.IsLateDateStart) return true;
        return false;
    }

    #endregion


}
