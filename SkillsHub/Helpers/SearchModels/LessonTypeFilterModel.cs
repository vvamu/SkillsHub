namespace SkillsHub.Helpers.SearchModels;

public class LessonTypeFilterModel
{
    public string? Name { get; set; } //filter-name

    public Guid? SubjectId { get; set; } //filter-subjectId
    public Guid? CourseId { get; set; } //filter-courseId
    public Guid? GroupTypeId { get; set; } //filter-groupTypeId

    public string? LocationType { get; set; } //filter-locationType
    public Guid? AgeTypeId { get; set; }  //filter-ageTypeId

    public string? IsDeleted { get; set; } //filter-isDeleted

}