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
            //.Include(x => x.Group).ThenInclude(x => x.GroupStudents).ThenInclude(x => x.ApplicationUser)
            
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
            .Include(x => x.Group).ThenInclude(x => x.GroupStudents).ThenInclude(x => x.Student)
            .Include(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.ArrivedStudents).ThenInclude(x => x.Student).ThenInclude(x => x.ApplicationUser).AsNoTracking()
            .FirstOrDefault(x => x.Id == id);//?? throw new Exception("Lesson not found");
        return lesson;

    }
    #endregion

    #region LessonStudents

    private async Task<List<LessonStudent>> CreateLessonStudents(List<Lesson> lessons, List<Guid> studentIds, Teacher teacher)
    {
        List<LessonStudent> lessonStudents = new List<LessonStudent>();

        foreach (var studentId in studentIds)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == studentId);
            //if id in DbSet<ApplicationUsers>
            if (student == null)
            {
                var userr = await _context.ApplicationUsers.Include(x => x.UserStudent).FirstOrDefaultAsync(x => x.Id == studentId);
                if (userr != null) student = userr.UserStudent;
            }

            var lessonStudent = new LessonStudent() { Student = student };

            lessonStudents.Add(lessonStudent);

            //----------------------------------------------------------------------------------------


            if (student.Lessons != null) student.Lessons.Add(lessonStudent);
            else student.Lessons = new List<LessonStudent> { lessonStudent };

            //Для каждого студента список занятий = все занятия
            foreach (var lesson in lessons)
            {
                lessonStudent.Lesson = lesson;
            }
            await _context.LessonStudents.AddAsync(lessonStudent);
            _context.Students.Update(student);



        }
        //----------------------------------------------------------------------------------------
        //Для каждого занятия список ученико = все ученики
        foreach (var lesson in lessons)
        {
            lesson.ArrivedStudents = lessonStudents;
            _context.Lessons.Update(lesson);


        }



        return lessonStudents;
    }

   

    #endregion

}
