using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class UserDaySchedule : BaseEntity
{
    public string DayName { get; set; }
    public ApplicationUser User { get; set; }
    public DateTime WorkingStartTime { get; set; }
    public DateTime WorkingEndTime { get; set; }

}
