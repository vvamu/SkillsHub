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
    public LessonType? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public string Name { get; set; } 
	public int LessonTimeInMinutes { get; set; }
	public int MinumumLessonsToPay { get; set; }
	public decimal StudentPrice { get; set; }
	public decimal TeacherPrice { get; set; }
	
    public Course? Course { get; set; } public Guid? CourseId { get; set; }
    public GroupType? GroupType { get; set; } public Guid? GroupTypeId { get; set; }
    public Location? Location { get; set; } public Guid? LocationId { get; set; }
    public AgeType? AgeType { get; set; } public Guid? AgeTypeId { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateCreated { get; set; }


    public decimal StudentPricePerCource => StudentPrice * MinumumLessonsToPay;

    public decimal TeacherPricePerCource => TeacherPrice * MinumumLessonsToPay;

    public List<LessonTypeStudent>? LessonTypeStudents;
    public List<LessonTypeTeacher>? LessonTypeTeachers;

    public LessonType LessonType { get; set; }
    public Guid LessonTypeId { get; set; }
}



//Относится к одному занятию
//Администратор создает курс.
//Курс
//Выбирает по чем занятие - английский для взрослых, программирование для детей и тд
//Выбирает онлайн или оффлайн. +++ доделать возможность добавить добавлять кабинеты
//Выбирает количество занятий, которое должно быть
//Выбирает дни недели, в которое проводится занятие
//Появляется Период 01.11, 02.11, 05.11. Можно редактировать
//Выбирает размер оплаты
//Выбирает преподавателя
//Выбирает учеников/группу

//Занятие
//Онлайн или оффлайн
//Тип