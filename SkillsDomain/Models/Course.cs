using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class Course : BaseEntity
{
    public string? Name { get; set; }
    public Nullable<Guid> ParentId { get; set; }
    public Subject? Subject { get; set; }
    public Nullable<Guid> SubjectId { get; set; }
    public string? Description { get; set; }

    public string? LocationType { get; set; }

    public List<PossibleCourseTeacher>? PossibleCourseTeachers { get; set; }

    public List<PreferenceCourseStudent>? PreferenceCourseStudents { get; set; }

    public List<LessonTypeCourse>? LessonTypeCourses { get; set; }

    //public List<CourseAgeType>? CourseAgeTypes { get; set; }


}
