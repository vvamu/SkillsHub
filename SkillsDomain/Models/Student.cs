using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Student : BaseEntity
{
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }
    public string? EnglishLevel { get; set; }
    public bool EnglishLevelConfirmed { get; set; } = false;
    public string? IsRecorded { get; set; }
    public int CountPayedLessons { get; set; }
    public List<Group>? Groups { get; set; }
    public List<Lesson> Lessons { get; set; }
}


//?
public class  PaymentType
{

}

