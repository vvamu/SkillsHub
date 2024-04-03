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

    public string FirstName { get; set; }
    public string LastName { get; set; }

    [DefaultValue("Default Surname")]
    public string Surname { get; set; }

    [DefaultValue("Male")]
    public string Sex { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime BirthDate { get; set; }

    [NotMapped]
    public int Age
    {
        get
        {
            return (DateTime.Today.Year - this.BirthDate.Year);
        }
    }
    public string? Phone { get; set; }
    public string? Email { get; set; }
 
    [NotMapped]
    public string Password { get; set; } //?

    public string OwnHashedPassword { get; set; }
    
    public virtual Teacher? UserTeacher { get; set; }
    public virtual Student? UserStudent { get; set; }

    public List<NotificationUser>? Notifications { get; set; }

    public string? SourceFindCompany { get; set; }
    public List<ExternalConnection>? ExternalConnections { get; set; }

    [DefaultValue("A1")]
    public string? EnglishLevel { get; set; } = "A1";
    [DefaultValue("0")]
    public bool IsVerified { get; set; } = false; //need to del
    [DefaultValue("0")]
    public bool IsDeleted { get; set; } = false;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DefaultValue("CONVERT(date, GETDATE())")]
    public DateTime? RegistrationDate { get; set; } = DateTime.Now;

    

    // public Guid EnglishLevelId { get; set; }


}


