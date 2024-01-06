using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using System.Linq;

namespace SkillsHub.Helpers.SearchModels;

public class UserFilterModel
{
    public Guid ApplicationUserId { get; set; }
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }

    public int MinCountPayedLessons { get; set; }

    public int MinCountGroups { get; set; }
    public Guid GroupId { get; set; }


    public string WorkingDay { get; set; }

    public string? PossibleCourse { get; set; }

    public int IsDeleted { get; set; } = -100;
}



public class UserOrderModel
{
    public int CountPayedLessons { get; set; } = -100;
    public int CountGroups { get; set; } = -100;
   // public int CountLessons { get; set; } = -100;

}
