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
    public string? Name { get; set; }
    //public string? DisplayedName { get => Subject + " " + CourseName + " " + LocationType + " " + GroupType +}
    public LessonType? Parent { get; set; }
    public Nullable<Guid> ParentId { get; set; }
    public PaymentCategory? PaymentCategory { get; set; }
    [ForeignKey("PaymentCategory")]
    public Nullable<Guid> PaymentCategoryId { get; set; }

    public List<LessonTypeCourse>? LessonTypeCourses { get; set; }
    public List<LessonTypeLocationType>? LessonTypeLocationTypes { get; set; }
    public List<LessonTypeAgeType>? LessonTypeAgeTypes { get; set; }
    public List<LessonTypeGroupType>? LessonTypeGroupTypes { get; set; }
    public bool IsActive { get; set; } = true;

}


public class LessonTypeCourse
{
    public Course Course { get; set; }
    public Guid CourseId { get; set; }

    public LessonType LessonType { get; set; }
    public Guid LessonTypeId { get; set; }
}

public class LessonTypeLocationType
{
    public LocationType LocationType { get; set; }
    public Guid LocationTypeId { get; set; }

    public LessonType LessonType { get; set; }
    public Guid LessonTypeId { get; set; }
}


public class LessonTypeAgeType
{
    public AgeType AgeType { get; set; }
    public Guid AgeTypeId { get; set; }

    public LessonType LessonType { get; set; }
    public Guid LessonTypeId { get; set; }
}

public class LessonTypeGroupType
{
    public GroupType GroupType { get; set; }
    public Guid GroupTypeId { get; set; }

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