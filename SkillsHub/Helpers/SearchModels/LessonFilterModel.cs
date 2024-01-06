using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using System.Linq;

namespace SkillsHub.Helpers.SearchModels;
public class LessonFilterModel
{
    public string? Topic { get; set; }
    public Guid TeacherId { get; set; }
    public Guid StudentId { get; set; }
    public Guid GroupId { get; set; }
    public DateTime? MinDateCreated { get; set; }


}



public class LessonOrderModel
{
    public int DateCreated { get; set; } = -100;
    public int CountPayedLessons { get; set; } = -100;
    public int CountGroups { get; set; } = -100;
    public int CountCources { get; set; } = -100;

    // public int CountLessons { get; set; } = -100;

}
