namespace SkillsHub.Domain.Models;

public class Lesson
{
    public DateTime Date { get; set; }

    public TeacherDTO Teacher { get; set; }
    public List<StudentDTO> ArrivedStudent { get; set; }
    public bool IsСompleted { get; set; }
    public LessonType Type { get; set; }

}
