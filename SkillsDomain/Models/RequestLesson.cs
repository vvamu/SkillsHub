using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class RequestLesson : BaseEntity
{
    public ApplicationUser? User { get; set; }
    public Lesson? LessonBefore { get; set; }
    public string RequestMessage { get; set; }
    public string? AnswerStatus { get; set; }
    public DateTime NewStart { get; set; }
    public DateTime NewEnd { get; set; }
}
