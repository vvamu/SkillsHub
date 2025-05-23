﻿using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Interfaces;

public interface IGroupService
{
    public IQueryable<Group> GetItems();
    public IQueryable<Group> GetAllGroupsList();
    public Task<Group> GetAsync(Guid id);
    public Task<Group> GetAsyncToGroupsList(Guid id);
    public Task<Group> UpdateAsync(Group item, Guid[] studentId, string[] dayName, TimeSpan[] startTime);
    public Task<Group> CreateAsync(Group item, Guid[] studentId, string[] dayName, TimeSpan[] startTime);
    public Task<Group> DeleteAsync(Guid id, bool isHardDelete);
    public Task<List<GroupWorkingDay>> CreateScheduleDaysToGroup(Group item, string[] dayName, TimeSpan[] startTime);
    public Task<List<Lesson>> CreateLessonsBySchedule(List<GroupWorkingDay> schedules, DateTime startDate, int countLessons, Group group, bool isVerified);

    public Task<Group> UpdateGroupStudents(Group oldGroup, Group newGroup, List<Guid> studentsId);
    public Task<Group> UpdateGroupTeachers(Group oldGroup, Group newGroup, List<Guid> teacherId);


}
