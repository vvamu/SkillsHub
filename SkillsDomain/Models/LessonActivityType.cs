using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class LessonActivityType : BaseEntity
{
    public string Name { get; set; } //
    public decimal StudentPrice { get; set; }
    public decimal TeacherPrice { get; set; }
}
