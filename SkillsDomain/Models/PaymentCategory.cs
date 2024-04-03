using Microsoft.EntityFrameworkCore;
using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class PaymentCategory : BaseEntity
{
    public PaymentCategory? Parent { get; set; }
    public string? Name { get; set; } = string.Empty;
    public int MinCountLessons { get; set; }
    public int MinCountLessonsToPay { get; set; }

    [Precision(15, 4)]
    public decimal StudentPrice { get; set; }
    [Precision(15, 4)]
    public decimal TeacherPrice { get; set; }

    public int LessonTimeInMinutes { get; set; }
    public int LessonsPerWeek { get; set; }
    public int MonthDuration { get; set; }
    public List<Group>? Groups { get; set; }


}
