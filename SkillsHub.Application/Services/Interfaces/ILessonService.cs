using SkillsHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.Services.Interfaces;

public interface ILessonService
{
    public IQueryable<Lesson> GetAll();
    public IQueryable<Lesson> GetAllByFilter(string? filterStr, Guid? filterCourseId);
    public Task<Lesson> GetAsync(Guid id);

    public Task<Lesson> Create(Lesson lesson);
    public Task<Lesson> Edit(Lesson lesson);


    public Task DeleteLessonByGroup(Group group, Lesson lesson);
    public Task<List<LessonStudent>> UpdateLessonStudents(Lesson lesson, List<Guid> studentIds);
    //public Task<LessonStudent> UpdateLessonStudent(Lesson lesson, Guid studentId);

   // public Task<List<LessonStudent>> CreateLessonStudents(List<Lesson> lessons, List<Guid> studentIds, Teacher teacher);



}
