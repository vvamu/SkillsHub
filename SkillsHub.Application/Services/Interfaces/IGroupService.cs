using SkillsHub.Application.PageViewModel;
using SkillsHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.Services.Interfaces;

public interface IGroupService
{
    public IQueryable<Group> GetAll();
    public IQueryable<Group> GetAllByFilter(string? filterStr, Guid? filterCourseId);
    public Task<Group> GetAsync(Guid id);
    public Task<Group> EditAsync(Group item);
    public Task<Group> CreateAsync(Group item);
    public Task<Group> HardDeleteAsync(Guid id); //with lessons
    public Task<Group> CreateScheduleDaysToGroup(Group item, string[] dayName, TimeSpan[] startTime, Guid[] studentIds);
    public Task<List<Lesson>> CreateLessonsBySchedule(List<WorkingDay> schedules, DateTime startDate, int countLessons,Group group, bool isVerified);

    public Task<Group> UpdateStudentsInGroup(Group item, List<Guid> studentIds);



}
