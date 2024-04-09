using SkillsHub.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace SkillsHub.Domain.BaseModels;

public class BaseUserInfo : BaseEntity
{
    public string? Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; } = "Default Surname";
    public string Sex { get; set; } = "Male";



    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime BirthDate { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public List<ExternalConnection>? ExternalConnections { get; set; }

    public string? AdditionalInfo { get; set; }
    public List<ApplicationUserBaseUserInfo> ApplicationUserBaseUserInfo { get; set; }


}