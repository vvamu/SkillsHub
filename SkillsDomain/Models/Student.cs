using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Student : BaseEntity
{
    public ApplicationUser ApplicationUser { get; set; }
    public Guid ApplicationUserId { get; set; }
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }
    public string? EnglishLevel { get; set; }
    public bool EnglishLevelConfirmed { get; set; } = false;

    //check
    public string? IsRecorded { get; set; }

    //Payment
    public int CountPayedLessons { get; set; }
    //public Dictionary<int,DateTime> Payment {  get; set; }

    public List<CourceName>? PossibleCources { get; set; }
    public List<Group>? Groups { get; set; }
    public List<Lesson>? Lessons { get; set; }

    public List<WorkingDay>? WorkingDays {get;set;}


}


public class WorkingDay : BaseEntity
{
    public string DayName { get; set; }
    public DateTime? StartWorkingDate { get; set; }
    public DateTime? EndWorkingDate { get; set; }
}


//?
public class  PaymentType
{

}

