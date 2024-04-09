using SkillsHub.Domain.Models;

namespace SkillsHub.Domain.BaseModels;

public class ApplicationUserBaseUserInfo : BaseEntity
{
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public bool IsBase {  get; set; }

    public Guid BaseUserInfoId { get; set; }
    public BaseUserInfo BaseUserInfo { get; set; }
}