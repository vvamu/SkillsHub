using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Student : BaseEntity
{
    public ApplicationUser ApplicationUser { get; set; }
    public Guid ApplicationUserId { get; set; }
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }
    //Payment
    public int CountPayedLessons { get; set; }
    //public Dictionary<int,DateTime> Payment {  get; set; }

    public List<CourseNameStudent>? PossibleCources { get; set; }

    public List<GroupStudent>? Groups { get; set; }
    public List<LessonStudent>? Lessons { get; set; }

    public string? WorkingDays { get; set; }

    [NotMapped]
    public decimal CurrentCalculatedPrice
    {
        get
        {
            decimal res = 0;
            if (this.Groups == null || this.Groups.Count() == 0) return res;


            foreach (var group in this.Groups.Select(x=>x.Group))
            {
                decimal pricePerLesson = 0;
                if (group.LessonType == null) continue;
                pricePerLesson = group.LessonType.StudentPrice;
                int countPaidLessons = 0;
                foreach (var lesson in group.Lessons.Where(x => x.ArrivedStudents.Select(x=>x.Id).Contains(this.Id)))
                {
                    if (lesson.IsСompleted)
                        countPaidLessons++;
                }
                res += pricePerLesson * countPaidLessons;

            }
            return res;
        }
    }

    [NotMapped]
    public decimal TotalCalculatedPrice
    {
        get
        {
            decimal res = 0;
            if (this.Groups == null || this.Groups.Count() == 0) return res;


            foreach (var group in this.Groups.Select(x => x.Group))
            {
                decimal pricePerLesson = 0;
                if (group.LessonType == null) continue;
                pricePerLesson = group.LessonType.StudentPrice;
                int countPaidLessons = 0;
                foreach (var lesson in group.Lessons.Where(x => x.ArrivedStudents.Select(x => x.Id).Contains(this.Id)))
                {
                    if (lesson.IsСompleted)
                        countPaidLessons++;
                }
                res += pricePerLesson * countPaidLessons;

            }
            return res;
        }
    }

    [NotMapped]
    public List<Lesson> VisitedLessons
    {
        get
        {
            List<Lesson> lessons = new List<Lesson>();
            if(this.Groups == null || this.Groups.Select(x=>x.Group).Where(x=>x.Lessons != null).Count() == 0) return lessons;
            lessons = this.Groups.Select(x => x.Group).Where(x => x.Lessons != null).SelectMany(x=>x.Lessons).Where(x=>x.IsСompleted).Where(x=>x.ArrivedStudents != null && x.ArrivedStudents.Select(x=>x.Student.Id).Contains(this.Id)).ToList();

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



}
//?
public class  PaymentType
{

}