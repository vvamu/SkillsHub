using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class UserWorkingDay : WorkingDay
{
    public Guid? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

}
