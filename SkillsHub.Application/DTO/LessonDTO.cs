using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.DTO;

public class LessonDTO
{
    public List<Teacher> Teachers { get; set; }
    public List<LessonStudent> ArrivedStudent { get; set; }
    public Group? Group { get; set; }
    public bool IsСompleted { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    //public LessonType LessonType { get; set; }
    public string? Location { get; set; } //Online Offline



    public ApplicationUser Creator { get; set; }

}
