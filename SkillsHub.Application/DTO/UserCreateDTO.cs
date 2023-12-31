using SkillsHub.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace SkillsHub.Application.DTO;


public class UserCreateDTO
{
    public Guid Id { get; set; }
    public string Login { get; set; } //unique
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; } = "Default Surname";
    public string Username { get; set; }
    public string Sex { get; set; } = "Male";

    //[DataType(DataType.Date)]
    //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime BirthDate { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? SourceFindCompany { get; set; }
    public string Password { get; set; } //?

    public bool IsTeacher { get; set; }
    public bool IsStudent { get; set; }

    public List<Group> Groups { get; set; }

    public string? EnglishLevel { get; set; }

    public Guid EnglishLevelId {  get; set; }
    public bool EnglishLevelConfirmed { get; set; } = false;

    public bool IsVerified { get; set; } = false;

}


