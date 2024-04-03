using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class AgeType : BaseEntity
{
    public string? Name { get; set; }

    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public List<LessonTypeAgeType>? LessonTypeAgeTypes { get; set; }


}

