using System.ComponentModel;

namespace SkillsHub.Domain.Models;

public class GroupType : BaseModels.BaseEntity
{
	public string? Name { get; set; }
	[DefaultValue("1")]
	public int MinCountStudents { get; set; }
	[DefaultValue("1")]
	public int MaxCountStudents { get; set; }
	public List<PossibleCourseTeacher>? PossibleCourseTeachers { get; set; }
    public List<PreferenceCourseStudent>? PreferenceCourseStudents { get; set; }
    public List<LessonTypeGroupType>? LessonTypeGroupTypes { get; set; }

}
