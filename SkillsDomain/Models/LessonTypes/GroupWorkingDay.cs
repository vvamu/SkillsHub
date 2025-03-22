namespace SkillsHub.Domain.Models;

public class GroupWorkingDay : WorkingDay
{
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }

    public override bool Equals(object obj)
    {
        var other = obj as GroupWorkingDay;
        if (other == null) return false;
        return DayName == other.DayName
            && GroupId == other.GroupId
            && WorkingStartTime == other.WorkingEndTime
            && WorkingEndTime == other.WorkingEndTime
            && ((StartDate == other.StartDate
            && EndDate == other.EndDate) || (StartDate == null && EndDate == null));
    }
}
