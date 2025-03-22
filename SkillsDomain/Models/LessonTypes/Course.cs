using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Course : LessonTypePropertyModel<Course>, IUploadImageModel
{
    public string Subject { get; set; }
    [NotMapped]
    public string SubjectRu
    {
        get
        {
            if (string.IsNullOrEmpty(Subject)) return "Не задано";
            switch (Subject)
            {
                case "English": return "Английский";
                case "Programming": return "Программирование";
                default: return "Другое";
            }
        }
    }

    //public string SubjectCode { get; set; }
    public string? Description { get; set; }
    #region MainPage

    public int OrderOnMainPage { get; set; }
    public string? PathToImage { get; set; }
    public string? PathToIcon { get; set; }

    [NotMapped]
    public List<IFormFile>? ImagesFiles { get; set; }

    [NotMapped]
    public List<IFormFile>? IconsFiles { get; set; }

    public string? IdentityString { get; set; } 
    public bool IsVisibleOnMainPage { get; set; } = true;

    public string? DescriptionOnMainPage { get; set; }

    [NotMapped]

    public Microsoft.AspNetCore.Html.HtmlString? DescriptionOnMainPageHtml
    {
        get
        {

            if (string.IsNullOrEmpty(DescriptionOnMainPage)) return new HtmlString("");

            //var firstSymbol = DescriptionOnMainPage.Substring(0, 2).ToString();
            var str = DescriptionOnMainPage.Substring(2).ToString().Replace("\n","").Replace("----", "<br><br>").Replace("--", "<hr/>").Replace("- -", "<br/> -").Replace("-", "<br/>-").Replace("<hr/><br/>", "<hr/>");
            var val = new Microsoft.AspNetCore.Html.HtmlString(str);
            return val;
        }
    }

    public string? DescriptionOnMainPageMore { get; set; }

    [NotMapped]

    public Microsoft.AspNetCore.Html.HtmlString? DescriptionOnMainPageMoreHtml
    {
        get
        {
            return new Microsoft.AspNetCore.Html.HtmlString(DescriptionOnMainPageMore);
        }

    }

    [NotMapped]
    public string PositionOnSite { get; set; } = "right";

    [NotMapped]
    public bool IsEditMode { get; set; } = false;

    [NotMapped]
    public string? TitleToWebSite { get => string.IsNullOrEmpty(Name) ? "" : Name.Contains("\"") ? Name.Replace("\"", "") : this?.SubjectRu + " " + Name; }

    #endregion

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Course other = (Course)obj;
        return Name == other.Name && Subject == other.Subject &&
            Description == other.Description && OrderOnMainPage == other.OrderOnMainPage &&
            IdentityString == other.IdentityString && IsVisibleOnMainPage == other.IsVisibleOnMainPage;
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