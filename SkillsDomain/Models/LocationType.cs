using System.ComponentModel;

namespace SkillsHub.Domain.Models;

public class LocationType : BaseModels.BaseEntity
{
	public Guid? ParentId { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
    public List<PossibleCourseTeacher>? PossibleCourseTeachers { get; set; }

    public List<PreferenceCourseStudent>? PreferenceCourseStudents { get; set; 
    public List<LessonTypeLocationType>? LessonTypeLocationTypes { get; set; }

}
