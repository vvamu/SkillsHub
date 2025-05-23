﻿using Microsoft.AspNetCore.Identity;
using SkillsHub.Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace SkillsHub.Domain.BaseModels;

public class ApplicationUser : IdentityUser<Guid>
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

    public List<ApplicationUserBaseUserInfo>? ConnectedUsersInfo { get; set; }

    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DefaultValue("CONVERT(date, GETDATE())")]
    public DateTime? RegistrationDate { get; set; } = DateTime.Now;

    public List<NotificationUser>? Notifications { get; set; }
    public List<UserWorkingDay>? UserWorkingDays { get; set; }

    public string OwnHashedPassword { get; set; }
    [DefaultValue("0")]
    public bool IsDeleted { get; set; } = false;
    [ForeignKey("UserTeacherId")] public virtual Teacher? UserTeacher { get; set; }
    public Guid? UserTeacherId { get; set; }
    [ForeignKey("UserStudentId")] public virtual Student? UserStudent { get; set; }
    public Guid? UserStudentId { get; set; }


    #region NotMapped

    [NotMapped]
    public string? RolesString { get => Roles == null ? "" : String.Join(";", Roles); }
    [NotMapped]
    public string[]? Roles { get; set; }

    [NotMapped]
    public string? Password { get; set; } //?

    [DefaultValue("0")]
    public bool IsVerified { get; set; } = false; //need to del

    [NotMapped]
    public int Age { get { if (UserInfo != null) return (UserInfo.Age); else return 0; } }

    [NotMapped]
    public BaseUserInfo? UserInfo
    {
        get => ConnectedUsersInfo?.OrderByDescending(x=>x.DateCreated).FirstOrDefault(x => x.IsBase == true)?.BaseUserInfo;
        /*
        set
        {
            var ConnectedUsersInfo = this.ConnectedUsersInfo ?? new List<BaseUserInfo>();
            ConnectedUsersInfo.Add(new BaseUserInfo());
        }*/
    }

    [NotMapped]
    public string? FullName { get => $"{UserInfo?.FullName}"; }

    //-----
    [NotMapped]
    public string? FirstName { get => UserInfo?.FirstName; }
    [NotMapped]
    public string? MiddleName { get => UserInfo?.MiddleName; }
    [NotMapped]
    public string? Surname { get => UserInfo?.Surname; }
    [NotMapped]
    public string? Sex { get => UserInfo?.Sex;  }
    [NotMapped]
    public DateTime BirthDate { get { if (UserInfo == null) return DateTime.Now; else return UserInfo.BirthDate; } }

    [NotMapped]
    public string? Phones { get => UserInfo?.Phones; }

    [NotMapped]
    public string[] PhonesArray { get => UserInfo?.PhonesArray; }
    [NotMapped]
    public string[] EmailsArray { get => UserInfo?.EmailsArray; }
    [NotMapped]
    public string EnglishLevel { get => UserInfo?.EnglishLevel; }

    [NotMapped]
    public string? Email { get => UserInfo?.Email; set { var one = UserInfo?.EmailsArray.First(); one = value; } }
    [NotMapped]
    public string? Phone { get => UserInfo?.Phone; set { var one = UserInfo?.PhonesArray.First(); one = value; } }

    #endregion


}


