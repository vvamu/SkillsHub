using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
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


    public async Task<List<LessonStudent>> UpdateLessonStudents(Lesson lesson, List<Guid> studentsId)
    {
        var groupName = "";
        if (lesson.Group != null) groupName = lesson.Group.Name;
        List<LessonStudent> lessonStudentsByGroup = lesson.ArrivedStudents.ToList();

        /* TEST
        foreach (var studentId in studentsId)
        {
            //if (user != null) studentId = user.Id;

       
            var lessonStudent = _context.LessonStudents.
                FirstOrDefault(x=>x.StudentId == studentId && x.LessonId == lesson.Id) 
                ?? new LessonStudent() { StudentId = studentId, LessonId = lesson.Id };

            try
            {
                _context.Entry(lessonStudent.Student).State = EntityState.Unchanged;
                _context.Entry(lessonStudent.Lesson).State = EntityState.Unchanged;
            }catch { }
        

            await _context.SaveChangesAsync();
        

            if (lessonStudent.Id != Guid.Empty) _context.LessonStudents.Update(lessonStudent);
            else await _context.LessonStudents.AddAsync(lessonStudent);
        

            await _context.SaveChangesAsync();
        }
        */

        if (lesson.Group != null && lesson.Group.IsPermanentStaffGroup && lesson.IsСompleted) return lessonStudentsByGroup;

        try
        {
            if (lesson != null && lesson.ArrivedStudents != null)
            {
                
                foreach (var student in lessonStudentsByGroup)
                {
                    if (!studentsId.Contains(student.Id))
                    {
                        _context.LessonStudents.Remove(student);

                        try
                        {
                            _context.Students.ToList();
                            _context.ApplicationUsers.ToList();
                            var usersToSend = new List<NotificationUser>() { new NotificationUser() { UserId = student.Student.ApplicationUser.Id } };
                            var message = " You was removed lesson by group " + groupName + "; Time: "  + lesson.StartTime.ToShortDateString() + " " + lesson.EndTime.ToShortDateString();
                            var notification = new NotificationMessage() { Message = message, Users = usersToSend };

                            usersToSend.ForEach(x => x.NotificationMessage = notification);
                            await _context.NotificationUsers.AddRangeAsync(usersToSend.AsEnumerable());
                            await _context.NotificationMessages.AddAsync(notification);
                            await _context.SaveChangesAsync();
                        }
                        catch { }
                    }
                    try
                    {
                        if (!studentsId.Contains(student.Id))
                        {
                            _context.Students.ToList();
                            _context.ApplicationUsers.ToList();
                            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserStudent.Id == student.Id);

                            var usersToSend = new List<NotificationUser>() { new NotificationUser() { UserId = user.Id } };
                            var message = " You was added to lesson in group " + groupName;
                            var notification = new NotificationMessage() { Message = message, Users = usersToSend };

                            usersToSend.ForEach(x => x.NotificationMessage = notification);
                            await _context.NotificationUsers.AddRangeAsync(usersToSend.AsEnumerable());
                            await _context.NotificationMessages.AddAsync(notification);
                            await _context.SaveChangesAsync();

                        }
                    }
                    catch { }

                    _context.Entry(student).State = EntityState.Detached;
                }

                await _context.SaveChangesAsync();
            }



            //add all
            foreach (var studentId in studentsId)
            {
                if (await _context.LessonStudents.FirstOrDefaultAsync(x => x.LessonId == lesson.Id && x.StudentId == studentId) != null) continue;
                var grSt = new LessonStudent() { LessonId = lesson.Id, StudentId = studentId };
                await _context.LessonStudents.AddAsync(grSt);
            }


            await _context.SaveChangesAsync();
        }
        catch (Exception ex) { }



        return lessonStudentsByGroup;
    }

    /*
    
        public async Task<List<LessonStudent>> UpdateLessonStudents(Lesson lesson, List<Guid> studentIds)
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

        ////
        if(lesson.ArrivedStudents != null &&  lesson.ArrivedStudents.Count() != 0)
        {
            foreach(var i in lesson.ArrivedStudents)
            {
                _context.LessonStudents.Remove(i);
            }
            await _context.SaveChangesAsync();
        }
    /////



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
        */

    public async Task DeleteLessonByGroup(Group group, Lesson lesson)
    {
        //_context.Entry(group.Lessons).State = EntityState.Unchanged;
        //var lesson2 = new Lesson() { Id = lesson.Id };
        var students = await _context.LessonStudents.Include(x=>x.Student).Where(x=>x.Lesson.Id == lesson.Id).ToListAsync();
        
        foreach (var student in students)
        {
            _context.Entry(student.Student).State = EntityState.Unchanged;
            _context.LessonStudents.Remove(student);
        }

        //group.Lessons.Remove(lesson2);
        _context.Lessons.Remove(lesson);
        //_context.Groups.Update(group);

        await _context.SaveChangesAsync();
    }

    /*
    public async Task DeleteLessonByGroup(Group group, Lesson lesson)
    {
        //_context.Entry(group.Lessons).State = EntityState.Unchanged;
        var lesson2 = new Lesson() { Id = lesson.Id };
        //if(group.Lessons != null)
        //_context.Entry(group.Lessons).State = EntityState.Detached;

        var students = lesson.ArrivedStudents;
        if(students != null)
        {
            foreach (var student in students)
            {
                _context.Entry(student.Student).State = EntityState.Unchanged;
                _context.LessonStudents.Remove(student);
                await _context.SaveChangesAsync();

            }
        }

        //group.Lessons.Remove(lesson2);
        //_context.Groups.Update(group);
        _context.Lessons.Remove(lesson2);

        await _context.SaveChangesAsync();
    }
    */

    #endregion

    public async Task<Lesson> Create(Lesson lesson)
    {
        var lessonValidator = new LessonValidator();
        var validationResult = await lessonValidator.ValidateAsync(lesson);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }

        #region CheckCorrect

        Guid groupId = (Guid)lesson.GroupId;
        if (lesson.Group != null) groupId = lesson.Group.Id;
        int duration = 0;
        var group = await GetGroupAsync(lesson.GroupId ?? groupId);
        if (group != null) duration = group.LessonType.LessonTimeInMinutes;
        if (lesson.EndTime.Minute - lesson.StartTime.Minute > duration * 2 || lesson.EndTime < lesson.StartTime) throw new Exception("Not correct date");

        var lessonsByGroup = _context.Lessons.Include(x => x.Group).Where(x => x.Group.Id == groupId).OrderBy(x => x.StartTime);
        foreach (var less in lessonsByGroup)
        {
            if (lesson.StartTime.CompareTo(less.StartTime) >= 0 && lesson.EndTime.CompareTo(less.EndTime) <= 0)
            {
                throw new Exception("New lesson time conflicted with lesson :" + less.StartTime + " - " + less.EndTime);
            }
        }

        #endregion

        #region AddStudentsToNewGroup
        if (group.IsPermanentStaffGroup)
        {
            // в идеале ассинхрон
            var students = group.GroupStudents.Select((x)=> new LessonStudent() { IsVisit = false, LessonId = lesson.Id, StudentId = x.StudentId } ).ToList();
            await _context.LessonStudents.AddRangeAsync(students);
        }
        #endregion

        await _context.Lessons.AddAsync(lesson);
        await _context.SaveChangesAsync();
        return lesson;
       
    }

    public async Task<Lesson> Edit(Lesson lesson)
    {
        var lessonValidator = new LessonValidator();
        var validationResult = await lessonValidator.ValidateAsync(lesson);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }

        #region CheckCorrect

        Guid groupId = (Guid)lesson.GroupId;
        if (lesson.Group != null) groupId = lesson.Group.Id;
        int duration = 0;
        var group = await GetGroupAsync(lesson.GroupId ?? groupId);
        if (group != null) duration = group.LessonType.LessonTimeInMinutes;
        if (lesson.EndTime.Minute - lesson.StartTime.Minute > duration * 2 || lesson.EndTime < lesson.StartTime) throw new Exception("Not correct date");

        var lessonsByGroup = _context.Lessons.Include(x => x.Group).Where(x => x.Group.Id == groupId).OrderBy(x => x.StartTime);
        foreach (var less in lessonsByGroup)
        {
            if (less.Id == lesson.Id) continue;
            if (lesson.StartTime.CompareTo(less.StartTime) >= 0 && lesson.EndTime.CompareTo(less.EndTime) <= 0)
            {
                throw new Exception("New lesson time conflicted with lesson :" + less.StartTime + " - " + less.EndTime);
            }
        }

        #endregion

        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();

        return lesson;

    }

    public async Task<Group> GetGroupAsync(Guid id)
    {
        var groups = await _context.Groups
            .Include(x => x.Lessons)
            .Include(x => x.Lessons).ThenInclude(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.Lessons).ThenInclude(x => x.ArrivedStudents).ThenInclude(x => x.Student).ThenInclude(x => x.ApplicationUser)


            .Include(x => x.Lessons).Include(x => x.CourseName).Include(x => x.LessonType)
            //.Include(x => x.GroupStudents).ThenInclude(x => x.Group)
            .Include(x => x.GroupStudents).ThenInclude(x => x.Student).ThenInclude(x => x.ApplicationUser)
            //.Include(x => x.GroupStudents).ThenInclude(x => x.Group).ThenInclude(x => x.Lessons)
            .Include(x => x.DaySchedules)
            .Include(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.CourseName)
            .Include(x => x.LessonType).ToListAsync();
        var item = groups.Find(x => x.Id == id);
        return item;
    }
}
