using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkillsHub.Domain.Models;

public class WorkingDay : BaseEntity
{
    public DayOfWeek? DayName { get; set; }
    public string? DayNameString { get => System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)DayName);}
    public TimeSpan? WorkingStartTime { get; set; }
    public TimeSpan? WorkingEndTime { get; set; }

    public TimeSpan? StartDate { get; set; }
    public TimeSpan? EndDate { get; set; }
    public string? RepeatIntervalName { get; set; } //День, Неделя, Месяц
    public string? RepeatIntervalValue { get; set; } 

}
