using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class GroupTeacher : BaseEntity
{
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }

}
