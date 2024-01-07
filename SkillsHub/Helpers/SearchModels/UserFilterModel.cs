using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using System.Linq;

namespace SkillsHub.Helpers.SearchModels;

public class UserFilterModel
{
    public string? FIO {  get; set; }
    public string? TeacherWorkingDay { get; set; }
    public string? StudentWorkingDay { get; set; }

    public string? TeacherPossibleCource { get; set; }
    public string? StudentPossibleCource { get; set; }

    public string IsDeleted { get; set; }


    //-----------------------
    /*
    public Guid ApplicationUserId { get; set; }
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }
    //


    //public int MinCountPayedLessons { get; set; }

   // public int MinCountGroups { get; set; }
    public Guid GroupId { get; set; }


    */
}



public class UserOrderModel
{
    public int CountPayedLessons { get; set; } = -100;
    public int CountGroups { get; set; } = -100;
   // public int CountLessons { get; set; } = -100;

}
