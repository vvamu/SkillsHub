using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Course : LessonTypePropertyModel<Course>
{
    public string Subject { get; set; }
    [NotMapped]
    public string SubjectRu { get
        {
            if (string.IsNullOrEmpty(Subject)) return "Не задано";
            switch(Subject)
            {
                case "English": return "Английский";
                case "Programming": return "Программирование";
                default: return "Другое";
            }
        }
    }

    //public string SubjectCode { get; set; }
    public string? Description { get; set; }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Course other = (Course)obj;
        return Name == other.Name && Subject == other.Subject;
    }

    [NotMapped]
    public override string? DisplayName { get => $"Предмет: {SubjectRu}; Направление: {Name}"; }

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