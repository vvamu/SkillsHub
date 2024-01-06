namespace SkillsHub.Domain.Models;

public class CourseNameTeacher
{
    public Guid CourseNameId { get; set; }
    public CourseName CourseName { get; set; }

    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }
}
