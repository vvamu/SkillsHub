using SkillsHub.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.BaseModels;

public class BaseUserInfo : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public string Surname { get; set; } 
    public string Sex { get; set; } = "Male";
    public bool IsBase { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime BirthDate { get; set; } = DateTime.Now.AddDays(-1 * 365 * 20);

    [NotMapped]
    public int Age { get => DateTime.Now.Year - BirthDate.Year;}
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public List<ExternalConnection>? ExternalConnections { get; set; }

    public string? AdditionalInfo { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public Guid? ApplicationUserId { get; set; }



}