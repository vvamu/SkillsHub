using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class UserDaySchedule : BaseEntity
{
    public DayOfWeek DayName { get; set; }
    public Group Group { get; set; }
    public TimeSpan WorkingStartTime { get; set; }
    public TimeSpan WorkingEndTime { get; set; }

}
