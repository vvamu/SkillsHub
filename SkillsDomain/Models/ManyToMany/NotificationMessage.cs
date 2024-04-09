using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SkillsHub.Domain.Models;

public class NotificationMessage : BaseEntity
{
    public string Message { get; set; }
    public List<NotificationUser>? Users { get; set; }

    public bool IsRequest {  get; set; }
    public ApplicationUser? Sender { get; set; }
}
