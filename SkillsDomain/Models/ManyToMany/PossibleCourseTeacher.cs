namespace SkillsHub.Domain.Models;

public class PossibleCourseTeacher
{
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public GroupType? GroupType { get; set; }
    public Nullable<Guid> GroupTypeId { get; set; }
    public LocationType? LocationType { get; set; }
    public Nullable<Guid> LocationTypeId { get; set; }
}
