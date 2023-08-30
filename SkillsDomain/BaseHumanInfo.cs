namespace SkillsDomain;

public class BaseHumanInfo : BaseEntity
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




}
