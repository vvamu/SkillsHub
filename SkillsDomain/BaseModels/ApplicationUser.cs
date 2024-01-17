using Microsoft.AspNetCore.Identity;
using SkillsHub.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SkillsHub.Domain.BaseModels;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Login { get; set; } //unique
    public override string? UserName { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; } = "Default Surname";
    public string Sex { get; set; } = "Male";



    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime BirthDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? SourceFindCompany { get; set; }
    public List<ExternalConnection>? ExternalConnections { get; set; }
    public string Password { get; set; } //?
    public string OwnHashedPassword { get; set; }
    public bool IsDeleted { get; set; } = false;
    public virtual Teacher? UserTeacher { get; set; }
    public virtual Student? UserStudent { get; set; }
    public bool IsVerified { get; set; } = false;

    public List<NotificationUser>? Notifications { get; set; }

    [NotMapped]
    public int Age { get
        {
            return (DateTime.Today.Year - this.BirthDate.Year);
        }
    }


    //public bool IsEditMode { get; set; } = false;


    //public string? EnglishLevel { get; set; }


    // public Guid EnglishLevelId { get; set; }


}


