using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.Models;

public class PreferenceCourseStudent
{
    

    public Guid StudentId { get; set; }
    public Student Student { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public GroupType? GroupType { get; set; }
    public Nullable<Guid> GroupTypeId { get; set; }
    public LocationType? LocationType { get; set; }
    public Nullable<Guid> LocationTypeId { get; set; }
}
