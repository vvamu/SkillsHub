using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class WorkingDay : BaseEntity
{
    public DayOfWeek? DayName { get; set; }
    public Group? Group { get; set; }
    //public Student? Student { get; set; }
    //public Teacher? Teacher { get; set; }
    public TimeSpan? WorkingStartTime { get; set; }
    public TimeSpan? WorkingEndTime { get; set; }

}
