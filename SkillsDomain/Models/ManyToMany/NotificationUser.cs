﻿using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class NotificationUser : BaseEntity
{
    public NotificationMessage NotificationMessage { get; set; }
    public Guid NotificationMessageId { get; set; }
    public ApplicationUser User { get; set; }
    public Guid UserId { get; set; }
}
