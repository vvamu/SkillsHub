using System.ComponentModel.DataAnnotations;

namespace SkillsHub.Domain.DTO;

public class UserLogin
{
    public string Login { get; set; }
    public string Password { get; set; }
}
