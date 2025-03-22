using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class UserWorkingDay : WorkingDay
{
    public Guid? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

}
