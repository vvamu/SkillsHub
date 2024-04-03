using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class Subject : BaseEntity
{
    public string? Name { get; set; }
    public Nullable<Guid> ParentId { get; set; }
    
    public List<Course>? Courses { get; set; }
}
