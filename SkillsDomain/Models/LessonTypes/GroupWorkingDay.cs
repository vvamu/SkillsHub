using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class GroupWorkingDay : WorkingDay
{
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
}
