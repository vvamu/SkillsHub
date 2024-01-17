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
    public Task<Group> GetAsync(Guid id);
    public Task<Group> EditAsync(Group item);
    public Task<Group> CreateAsync(Group item);
    public Task<Group> HardDeleteAsync(Guid id); //with lessons
    public Task<Group> CreateScheduleDaysToGroup(Group item, string[] dayName, TimeSpan[] startTime);
    public Task<List<Lesson>> CreateLessonsBySchedule(List<WorkingDay> schedules, DateTime startDate, int countLessons,Group group, bool isVerified);

    public Task<Group> UpdateGroupStudents(Group item, List<Guid> studentIds);



}
