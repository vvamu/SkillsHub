using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

//TODELETE
public class Cource : BaseModels.BaseEntity
{
    public List<Lesson> Lessons { get; set; }
    public CourseName Name { get; set; }
    //public EnglishLevel? EnglishLevel { get; set; } 

    //public string Term { get; set; }
    public string SubscriptionType { get; set; } //Term , Termless
    public int MaxArrivedStudents { get; set; }
    public int MinimumAge { get; set; }

    [NotMapped]
    public int LessonsPerWeek { get; set; }

    [NotMapped]
    public int CountMonth { get; set; }


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