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
    

}
//?
public class  PaymentType
{

}