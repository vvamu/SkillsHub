using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.DTO;

public class StudentDTO : ApplicationUser
{
    public List<LessonDTO> StudentLessons { get; set; }
    public List<LessonDTO> TeacherLessons { get; set; }

    public string? ParentPhone { get; set; }
    public string? ParentName { get; set; }

    public string? EnglishLevel { get; set; }
    public string IsRecorded { get; set; }
    public int CountPayedLessons { get; set; }
    public int VisitedLessons { get; set; }


    public List<Group>? Groups { get; set; }

    public IQueryable<Cource> Cources { get; set; }

}