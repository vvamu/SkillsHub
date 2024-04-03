using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class LessonStudent : BaseEntity
{
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; }
    public int VisitStatus { get;set; }  //1 - Visited 2 - Missed without saving lesson 3 - Missed  with saving lesson 
    public string LessonType { get; set; } = "Default"; //Отработка/Занятие/Групповое/Trial

   public string? Comment { get; set; }

}

//public class GroupStudent : BaseEntity
//{
//    public Guid GroupId { get; set; }
//    public Group Group { get; set; }
//    public Guid StudentId { get; set; }
//    public Student Student { get; set; }
//    public bool IsVisit { get; set; }

//}
