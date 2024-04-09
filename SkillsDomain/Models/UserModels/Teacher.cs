using SkillsHub.Domain.BaseModels;
using SkillsHub.Persistence;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Teacher : BaseEntity
{
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public List<GroupTeacher>? Groups { get; set; }
    public List<LessonTeacher>? Lessons { get; set; }
    public List<LessonTypeTeacher>? PossibleCources { get; set; }

    public string? WorkingDays { get; set; }
    public decimal PaidAmount { get; set; }

    [NotMapped]
    public decimal CurrentCalculatedPrice {  get; set; }
    [NotMapped]
    public decimal TotalCalculatedPrice { get; set; }
    
    /*
    [NotMapped]
    public List<Lesson> PassedLessons
    {
        get
        {
            List<Lesson> lessons = new List<Lesson>();
            if (this.Groups == null || this.Groups.Where(x => x.Lessons != null).Count() == 0) return lessons;
            lessons = this.Groups.Where(x => x.Lessons != null).SelectMany(x => x.Lessons).ToList();//.Where(x => x.Teacher != null && x.Teacher == this).ToList();

            List<Lesson> lessonsf = new List<Lesson>();
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
    [NotMapped]
    public List<Lesson> PreparedLessons
    {
        get
        {
            
            List<Lesson> lessons = new List<Lesson>();
            if (this.Groups == null || this.Groups.Select(x=>x.Group).Where(x => x.Lessons != null).Count() == 0) return lessons;
            lessons = this.Groups.Select(x=>x.Group).Where(x => x.Lessons != null).SelectMany(x => x.Lessons).ToList();//.Where(x => x.Teacher != null && x.Teacher == this).ToList();
            
            List<Lesson> lessonsf = new List<Lesson>();
            //if (this.Groups == null || this.Groups.Where(x => x.Lessons != null).Count() == 0) return lessons;

            foreach (var group in this.Groups.Select(x => x.Group))
            {
                foreach (var lesson in group.Lessons.Select(x=>x.Teacher).Where(x => x.Teacher == this).Select(x=>x.Lesson))
                {
                        lessons.Add(lesson);
                }

            }

            */
            return lessons;

        }
    }





}