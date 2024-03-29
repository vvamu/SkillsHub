﻿using Microsoft.AspNetCore.Identity;
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
    //private readonly IRequestService _requestService;
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationService _notificationService;

    public LessonService(ApplicationDbContext context, IUserService userService,UserManager<ApplicationUser> userManager,INotificationService notificationService)
    {
        _context = context;
       // _requestService = requestService;
        _userService = userService;
        _userManager = userManager;
        _notificationService = notificationService;
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
                            var user = await _context.ApplicationUsers.Include(x=>x.UserStudent).FirstOrDefaultAsync(x => x.UserStudent.Id == student.StudentId);

                            var usersToSend = new List<NotificationUser>() { new NotificationUser() { UserId = user.Id } };
                            var message = " You was removed from lesson by group " + groupName + "; Time: "  + lesson.StartTime.ToShortDateString() + " " + lesson.EndTime.ToShortDateString();
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
                            if(student != null)
                            {
                                var user = await _context.ApplicationUsers.Include(x=>x.UserStudent).FirstOrDefaultAsync(x => x.UserStudent.Id == student.StudentId);

                                var usersToSend = new List<NotificationUser>() { new NotificationUser() { UserId = user.Id } };
                                var message = " You was added to lesson in group " + groupName;
                                var notification = new NotificationMessage() { Message = message, Users = usersToSend };

                                usersToSend.ForEach(x => x.NotificationMessage = notification);
                                await _context.NotificationUsers.AddRangeAsync(usersToSend.AsEnumerable());
                                await _context.NotificationMessages.AddAsync(notification);
                                await _context.SaveChangesAsync();
                            }
                           

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

        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();
    }


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

        

        await _context.Lessons.AddAsync(lesson);
        await _context.SaveChangesAsync();

        #region AddStudentsToNewGroup
        if (group.IsPermanentStaffGroup)
        {
            // в идеале ассинхрон
            var students = group.GroupStudents.Select((x) => new LessonStudent() { IsVisit = false, LessonId = lesson.Id, StudentId = x.StudentId }).ToList();
            await _context.LessonStudents.AddRangeAsync(students);
        }

        await _context.SaveChangesAsync();
        #endregion

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
        /*foreach (var less in lessonsByGroup)
        {
            if (less.Id == lesson.Id) continue;
            if (lesson.StartTime.CompareTo(less.StartTime) >= 0 && lesson.EndTime.CompareTo(less.EndTime) <= 0)
            {
                throw new Exception("New lesson time conflicted with lesson :" + less.StartTime + " - " + less.EndTime);
            }
        }*/
        #endregion
        var user = await _userService.GetCurrentUserAsync();
        var prevLesson = await _context.Lessons.AsNoTracking().Include(x => x.Group).ThenInclude(x => x.LessonType).FirstOrDefaultAsync(x => x.Id == lesson.Id);
        _context.Entry(prevLesson).State = EntityState.Detached;




        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();

        if (await _userManager.IsInRoleAsync(user, "Teacher")) //условие для учителей
        {
            var requestMessage = user.Login.ToString()
            + " want to update current lesson with values: "
            + "\nStart time : " + lesson.StartTime.ToShortDateString() + " " + lesson.StartTime.ToShortTimeString()
            + "\nEnd time : " + lesson.EndTime.ToShortDateString() + " " + lesson.EndTime.ToShortTimeString();
            await CreateRequest(prevLesson, requestMessage, lesson, user);

           

        }

        await _notificationService.СreateToEditLesson(prevLesson, lesson, null);
        await _context.SaveChangesAsync();


        if (await _userManager.IsInRoleAsync(user, "Teacher")) //условие для учителей
        {
            lesson.StartTime = prevLesson.StartTime;
            lesson.EndTime = prevLesson.EndTime;
        }
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();

        return lesson;

    }
    public async Task<RequestLesson> CreateRequest(Lesson prevLesson, string requestMessage, Lesson newLesson, ApplicationUser user)
    {
        DateTime newStart = DateTime.Now;
        DateTime newEnd = DateTime.Now;
        if (newLesson != null)
        {
            var duration = (newLesson.EndTime - newLesson.StartTime).TotalMinutes;
            var defaultDuration = prevLesson.Group.LessonType.LessonTimeInMinutes;
            requestMessage += "\n| Default time to lesson type " + prevLesson.Group.LessonType.Name + " : " + defaultDuration
                + "\n | New duration : " + duration
                + "\n Difference : " + (defaultDuration - duration);

            newStart = newLesson.StartTime;
            newEnd = newLesson.EndTime;

        }
        else
        {
            requestMessage += " in group '" + newLesson.Group.Name + "' ";//+ "' lesson  was deleted by date:  ";
                                                                       // + lesson.StartTime.ToShortTimeString() + " - " + lesson.EndTime.ToShortTimeString();
        }

        var request = new RequestLesson() { LessonBefore = newLesson, RequestMessage = requestMessage, User = user, NewStart = newStart, NewEnd = newEnd };

        _context.Entry(request.User).State = EntityState.Unchanged;
        _context.Entry(request.LessonBefore).State = EntityState.Unchanged;

        _context.RequestLessons.Add(request);
        await _context.SaveChangesAsync();
        return request;
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
