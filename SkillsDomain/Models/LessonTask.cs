using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class LessonTask : BaseEntity
{
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; }
    public string Task { get; set; }
}
