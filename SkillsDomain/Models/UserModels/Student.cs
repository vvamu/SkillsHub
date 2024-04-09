using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Student : BaseEntity
{
    public ApplicationUser ApplicationUser { get; set; }
    public Guid ApplicationUserId { get; set; }
    //Payment
    public List<FinanceElement>? FinanceElements { get; set; }

    public List<LessonTypeStudent>? PossibleCources { get; set; }
    public List<GroupStudent>? Groups { get; set; }
    public List<LessonStudent>? Lessons { get; set; }

    public string? WorkingDays { get; set; }

    #region NotMapped

    [NotMapped]
    public decimal CurrentCalculatedPrice { get; set; }


    [NotMapped]
    public decimal TotalCalculatedPrice { get; set; }

    [NotMapped]
    public List<Lesson> VisitedLessons
    {
        get
        {
            List<Lesson> lessons = new List<Lesson>();
            if (this.Groups == null || this.Groups.Select(x => x.Group).Where(x => x.Lessons != null).Count() == 0) return lessons;
            lessons = this.Groups.Select(x => x.Group).Where(x => x.Lessons != null).SelectMany(x => x.Lessons).Where(x => x.IsСompleted).Where(x => x.ArrivedStudents != null && x.ArrivedStudents.Select(x => x.Student.Id).Contains(this.Id)).ToList();

            return lessons;
        }
    }

    [NotMapped]
    public List<Lesson> PreparedLessons
    {
        get
        {
            List<Lesson> lessons = new List<Lesson>();
            if (this.Groups == null || this.Groups.Select(x => x.Group).Where(x => x.Lessons != null).Count() == 0) return lessons;
            lessons = this.Groups.Select(x => x.Group).Where(x => x.Lessons != null).SelectMany(x => x.Lessons).Where(x => x.IsСompleted).Where(x => x.ArrivedStudents != null && x.ArrivedStudents.Select(x => x.Student.Id).Contains(this.Id)).ToList();
            return lessons;
        }
    }

    [NotMapped]
    public int CountPayedLessons { get; set; }


    #endregion





}
//?