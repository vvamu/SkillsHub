using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SkillsHub.Domain.BaseModels;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Login { get; set; } //unique
    public override string? UserName { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

    public List<ApplicationUserBaseUserInfo>? ConnectedUsersInfo { get; set; }
    [DefaultValue("A1")]
    public string? EnglishLevel { get; set; } = "A1";

    

    public string? SourceFindCompany { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DefaultValue("CONVERT(date, GETDATE())")]
    public DateTime? RegistrationDate { get; set; }
    public List<ExternalConnection>? ExternalConnections { get => UserInfo?.ExternalConnections; }
    public List<NotificationUser>? Notifications { get; set; }


    #region NotMapped


    [NotMapped]
    public string? Password { get; set; } //?
    public string OwnHashedPassword { get; set; }
    [DefaultValue("0")]
    public bool IsDeleted { get; set; } = false;
    public virtual Teacher? UserTeacher { get; set; }
    public virtual Student? UserStudent { get; set; }
    [DefaultValue("0")]
    public bool IsVerified { get; set; } = false; //need to del

    [NotMapped]
    public int Age { get { if (UserInfo != null) return (UserInfo.Age); else return 0; } }

    [NotMapped]
    public BaseUserInfo? UserInfo
    {
        get => ConnectedUsersInfo?.FirstOrDefault(x => x.IsBase == true)?.BaseUserInfo;
        /*
        set
        {
            var ConnectedUsersInfo = this.ConnectedUsersInfo ?? new List<BaseUserInfo>();
            ConnectedUsersInfo.Add(new BaseUserInfo());
        }*/
    }

    [NotMapped]
    public string? FullName { get => $"{UserInfo?.FullName}";}

    //-----
    [NotMapped]
    public string? FirstName { get => UserInfo?.FirstName; }
    [NotMapped]
    public string? LastName { get => UserInfo?.LastName; }
    [NotMapped]
    public string? Surname { get => UserInfo?.Surname; }
    [NotMapped]
    public string? Sex { get => UserInfo?.Sex; }
    [NotMapped]
    public DateTime BirthDate { get { if (UserInfo == null) return DateTime.Now; else return UserInfo.BirthDate; }  }

    [NotMapped]
    public string? Phones { get => UserInfo?.Phones; }

    [NotMapped]
    public string[] PhonesArray { get => UserInfo?.PhonesArray; }
    [NotMapped]
    public string[] EmailsArray { get => UserInfo?.EmailsArray; }

    [NotMapped]
    public string? Email { get => UserInfo?.Email; set { var one = UserInfo?.EmailsArray.First(); one = value; } }
    [NotMapped]
    public string? Phone { get => UserInfo?.Phone; set { var one = UserInfo?.PhonesArray.First(); one = value; } }

    #endregion
    

}


