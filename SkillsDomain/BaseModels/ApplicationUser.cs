using Microsoft.AspNetCore.Identity;

namespace SkillsHub.Domain.BaseModels;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; }
    public string Sex { get; set; }
    public DateOnly BirthDay { get; set; }

    public DateOnly RegistrationDate { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public string? SourceFindCources { get; set; }
    public List<string>? ExternalConnections { get; set; }
    public bool IsDeleted { get; set; }



}
