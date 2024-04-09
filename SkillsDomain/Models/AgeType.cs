using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class AgeType : BaseEntity
{
    public GroupType? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public string Name { get; set; }

    public int MinimumAge { get; set; }
    public int MaximumAge { get; set; }

    public List<LessonType>? LessonTypes { get; set; }

}

