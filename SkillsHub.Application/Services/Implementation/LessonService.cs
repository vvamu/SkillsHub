using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.Services.Implementation;

public class LessonService : ILessonService
{
    private readonly ApplicationDbContext _context;

    public LessonService(ApplicationDbContext context)
    {
        _context = context;
    }

    #region Get

    public IQueryable<Lesson> GetAll()
    {
        var items = _context.Lessons
            .Include(x => x.Group).ThenInclude(x => x.LessonType)
            .Include(x => x.Group).ThenInclude(x => x.CourseName)
            //.Include(x => x.Group).ThenInclude(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.Group).ThenInclude(x => x.GroupStudents).ThenInclude(x => x.Student).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
            
            .Include(x => x.ArrivedStudents).ThenInclude(x => x.Student).ThenInclude(x => x.ApplicationUser)
            
            .OrderBy(x => x.StartTime).AsQueryable();
        return items;
    }

    public IQueryable<Lesson> GetAllByFilter(string? filterStr, Guid? filterCourseId)
    {
        throw new NotImplementedException();
    }

    public async Task<Lesson> GetAsync(Guid id)
    {
        var lesson = _context.Lessons
            .Include(x => x.Group).ThenInclude(x => x.LessonType)
            .Include(x=> x.Group).ThenInclude(x=>x.Teacher).ThenInclude(x=>x.ApplicationUser)
            .Include(x => x.Group).ThenInclude(x => x.GroupStudents).ThenInclude(x => x.Student).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.ArrivedStudents).ThenInclude(x => x.Student).ThenInclude(x => x.ApplicationUser).AsNoTracking()
            .FirstOrDefault(x => x.Id == id);//?? throw new Exception("Lesson not found");
        return lesson;

    }
    #endregion

    #region LessonStudents
    
    public async Task<List<LessonStudent>> UpdateStudentsByLesson(Lesson lesson, List<Guid> studentIds)
    {
        List<LessonStudent> lessonStudents = new List<LessonStudent>();

        try
        {

        for(int i = 0; i < studentIds.Count; i++)
        {
                
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == studentIds[i]);
            if (user != null)  studentIds[i] = user.Id;
        }
      
        var groupStudentsIds = _context.Groups.Include(x => x.Lessons).FirstOrDefault(x => x.Lessons.Select(x => x.Id).Contains(lesson.Id)).GroupStudents.Select(x=>x.Id);

        /*
        if(lesson.ArrivedStudents != null &&  lesson.ArrivedStudents.Count() != 0)
        {
            foreach(var i in lesson.ArrivedStudents)
            {
                _context.LessonStudents.Remove(i);
            }
            await _context.SaveChangesAsync();
        }*/




        foreach (var studentId in groupStudentsIds)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == studentId);
            var lessonStudent = new LessonStudent() { Student = student , Lesson = lesson };

            _context.Entry(lessonStudent.Student).State = EntityState.Unchanged;
            _context.Entry(lessonStudent.Lesson).State = EntityState.Unchanged;

            if (studentIds.Contains(studentId))
                lessonStudent.IsVisit = true;
            else
                lessonStudent.IsVisit = false;

            _context.LessonStudents.Add(lessonStudent);
            lessonStudents.Add(lessonStudent);

        }
        await _context.SaveChangesAsync();

            //----------------------------------------------------------------------------------------

        }
        catch (Exception ex) { }
        return lessonStudents;
    }
        

    public async Task DeleteLessonByGroup(Group group, Lesson lesson)
    {
        _context.Entry(group.Lessons).State = EntityState.Unchanged;
        var lesson2 = new Lesson() { Id = lesson.Id };

        group.Lessons.Remove(lesson2);
        _context.Lessons.Remove(lesson2);

        _context.Groups.Update(group);

        await _context.SaveChangesAsync();
    }

    #endregion

}
