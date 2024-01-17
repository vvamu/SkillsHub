using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class LessonType : BaseEntity
{
    public string Name { get; set; } 
	public int LessonTimeInMinutes { get; set; }
	public int MinumumLessonsToPay { get; set; }
	public decimal StudentPrice { get; set; }
	public decimal TeacherPrice { get; set; }
	public DateTime DateCreated { get; set; }

	public int MinimumStudents  { get; set; }
	public int MaximumStudents { get; set;}

	public int MinCountLessons { get; set; }


    //[NotMapped]
    //public decimal StudentPricePerCource => StudentPrice * MinumumLessonsToPay;
    //[NotMapped]
    //public decimal TeacherPricePerCource => TeacherPrice * MinumumLessonsToPay;


}
