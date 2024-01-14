using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Teacher : BaseEntity
{
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public List<Group>? Groups { get; set; }
    public string? WorkingDays { get; set; }

    public List<CourseNameTeacher>? PossibleCources { get; set; }

    [NotMapped]
    public decimal CurrentCalculatedPrice
    {
        get
        {
            decimal res = 0;
            if (this.Groups == null || this.Groups.Count() == 0) return res;


            foreach (var group in this.Groups)
            {
                decimal pricePerLesson = 0;
                if (group.LessonType == null) continue;
                pricePerLesson = group.LessonType.TeacherPrice;
                int countPaidLessons = 0;
                foreach (var lesson in group.Lessons.Where(x => x.Teacher == this))
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
    public decimal PreparedCalculatedPrice
    {
        get
        {
            decimal res = 0;
            if (this.Groups == null || this.Groups.Count() == 0) return res;


            foreach (var group in this.Groups)
            {
                decimal pricePerLesson = 0;
                if (group.LessonType == null) continue;
                pricePerLesson = group.LessonType.TeacherPrice;
                int countPaidLessons = 0;
                foreach (var lesson in group.Lessons.Where(x => x.Teacher == this))
                {
                    //if (lesson.IsСompleted)
                        countPaidLessons++;
                }
                res += pricePerLesson * countPaidLessons;

            }
            return res;
        }
    }

    /*
    [NotMapped]
    public List<Lesson> PassedLessons
    {
        get
        {
            List<Lesson> lessons = new List<Lesson>();
            //if (this.Groups == null || this.Groups.Where(x => x.Lessons != null).Count() == 0) return lessons;

            
            foreach (var group in this.Groups)
            {
                foreach (var lesson in group.Lessons.Where(x => x.Teacher == this))
                {
                    if (lesson.IsСompleted)
                    lessons.Add(lesson);
                }

            }
            


            //lessons = this.Groups.Where(x => x.Lessons != null).SelectMany(x => x.Lessons).ToList();//.Where(x => x.IsСompleted).Where(x=>x.Teacher != null && x.Teacher == this).ToList();
            return lessons;
        }
    }

    [NotMapped]
    public List<Lesson> PreparedLessons
    {
        get
        {
            
            List<Lesson> lessons = new List<Lesson>();
            if (this.Groups == null || this.Groups.Where(x => x.Lessons != null).Count() == 0) return lessons;
            lessons = this.Groups.Where(x => x.Lessons != null).SelectMany(x => x.Lessons).ToList();//.Where(x => x.Teacher != null && x.Teacher == this).ToList();
            return lessons;
            

            List<Lesson> lessons = new List<Lesson>();
            //if (this.Groups == null || this.Groups.Where(x => x.Lessons != null).Count() == 0) return lessons;
            



            foreach (var group in this.Groups)
            {
                foreach (var lesson in group.Lessons.Where(x => x.Teacher == this))
                {
                        lessons.Add(lesson);
                }

            }


            return lessons;

        }
    }


*/


}