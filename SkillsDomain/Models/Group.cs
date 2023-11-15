using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class Group : BaseEntity
{
    public string Name;
    public List<LessonStudent> ArrivedStudents { get; set; }

    public string Term { get; set; }
    public LessonType LessonType { get; set; } //Offline , Online
    public List<Lesson> Lessons { get; set; }

    public Cource Cource { get; set; }

    [NotMapped]
    public double AverageAgeStudents { get; set; }

}
