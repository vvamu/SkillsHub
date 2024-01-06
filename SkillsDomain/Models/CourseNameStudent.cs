namespace SkillsHub.Domain.Models;

public class CourseNameStudent
{
    public Guid CourseNameId { get; set; }
    public CourseName CourseName { get; set; }

    public Guid StudentId { get; set; }
    public Student Student { get; set; }
}
