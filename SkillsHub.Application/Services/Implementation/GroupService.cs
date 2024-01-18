using Azure.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using SkillsHub.Persistence.Migrations;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Group = SkillsHub.Domain.Models.Group;

namespace SkillsHub.Application.Services.Implementation;

public class GroupService: IGroupService
{
    private readonly ApplicationDbContext _context;
    private readonly ILessonService _lessonService;

    public GroupService(ApplicationDbContext context, ILessonService lessonService)
    {
        _context = context;
        _lessonService = lessonService;
    }
    

    #region Get
    public IQueryable<Group> GetAll() => _context.Groups
        .Include(x => x.Lessons).ThenInclude(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
        .Include(x => x.Lessons).ThenInclude(x => x.ArrivedStudents).ThenInclude(x => x.Student).ThenInclude(x=>x.ApplicationUser)


        .Include(x => x.GroupStudents).ThenInclude(x=>x.Student).ThenInclude(x => x.ApplicationUser)
        .Include(x => x.GroupStudents)//.ThenInclude(x => x.Group).ThenInclude(x => x.Lessons)
        //.Include(x => x.GroupStudents).ThenInclude(x => x.Student).ThenInclude(x=>x.L)
        .Include(x => x.DaySchedules)
        .Include(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
        .Include(x => x.CourseName)
        .Include(x => x.LessonType)
        .AsQueryable();
    public async Task<Group> GetAsync(Guid id)
    {
        var groups = await _context.Groups
            .Include(x=>x.Lessons)
            .Include(x => x.Lessons).ThenInclude(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.Lessons).ThenInclude(x => x.ArrivedStudents).ThenInclude(x => x.Student).ThenInclude(x => x.ApplicationUser)


            .Include(x => x.Lessons).Include(x => x.CourseName).Include(x => x.LessonType)
            //.Include(x => x.GroupStudents).ThenInclude(x => x.Group)
            .Include(x => x.GroupStudents).ThenInclude(x => x.Student).ThenInclude(x=>x.ApplicationUser)
            //.Include(x => x.GroupStudents).ThenInclude(x => x.Group).ThenInclude(x => x.Lessons)
            .Include(x => x.DaySchedules)
            .Include(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.CourseName)
            .Include(x => x.LessonType).ToListAsync();
        var item = groups.Find(x => x.Id == id);
        return item;
    }

    #endregion

    #region Create
    public async Task<Group> CreateAsync(Group item)
    {
        if (item == null) throw new Exception("Not correct data for group");
        if (item.IsLateDateStart) item.DateStart = DateTime.MinValue;

        var userRegisterValidator = new GroupValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }

        

        //if (_context.Groups.FirstOrDefault(x => x.Name == item.Name) != null) throw new Exception("Group with such name already exist");

        var teacher = _context.Teachers.FirstOrDefault(x => x.Id == item.TeacherId);
        ApplicationUser? userr;
        if (teacher == null)
        {
            userr = await _context.ApplicationUsers.Include(x => x.UserTeacher).FirstOrDefaultAsync(x => x.Id == item.TeacherId);
            if (userr != null) teacher = userr.UserTeacher;
        }

        if (teacher != null)
        {
            var teacherGroups = teacher.Groups ?? new List<Group>();
            teacherGroups.Add(item);
            _context.Teachers.Update(teacher);
            item.Teacher = teacher;

        }

        await _context.Groups.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;

    }

    #endregion

    #region Helpers

    public async Task<List<Lesson>> CreateLessonsBySchedule(List<WorkingDay> schedules, DateTime startDate, int countLessons, Group group, bool isVerified)
    {
        var lessons = new List<Lesson>();
        var groupLessons = _context.Lessons.Include(x => x.Group).Where(x => x.GroupId == group.Id);
        try
        {
            var date = startDate;//hz
            if (countLessons < 0) countLessons = 100;
            for (int lesCount = 0, scCount = 0; lesCount < countLessons; lesCount++, scCount++)
            {
                if (scCount == schedules.Count)
                {
                    scCount = 0;
                    date = date.AddDays(7);
                }
                var addingDays = LessonMath.Mod((int)date.DayOfWeek, (int)schedules[scCount].DayName);
                var virtualDate = date.AddDays(addingDays);
                var lesson = new Lesson()
                {
                    StartTime = virtualDate + schedules[scCount].WorkingStartTime ?? DateTime.Now,
                    EndTime = virtualDate + schedules[scCount].WorkingEndTime ?? DateTime.Now,
                    GroupId = group.Id, ///
                    
                    //ArrivedStudents = lessonStudents,
                    
                    TeacherId = group.TeacherId
                };

                //_context.Entry(lesson.Teacher).State = EntityState.Unchanged;



                lessons.Add(lesson);
                //_context.Entry(lesson.Group).State = EntityState.Unchanged;



                if (groupLessons != null && !groupLessons.Any(x => x.StartTime == lesson.StartTime && x.EndTime == lesson.EndTime))
                {
                    _context.Lessons.Add(lesson);

                }

            }
            ///
            group.Lessons = lessons;
            try
            {
                //_context.Entry(group.Teacher).State = EntityState.Unchanged;
                //_context.Entry(group.Teacher.ApplicationUser).State = EntityState.Unchanged;
                await _context.SaveChangesAsync();
                _context.Groups.Update(group);

            } catch (Exception ex) { }
           

            try
            {
                
                var entry = _context.Entry<Group>(group);


                _context.Entry(group).State = EntityState.Unchanged;
                

            }
            catch {
                

            }


            await _context.SaveChangesAsync();


            //await AddStudentsToLessons(group, group.GroupStudents.Select(x => x.Id).ToList(), lessons);

        }



        catch (Exception ex) { }

        return lessons;


    }

    private async Task<bool> CheckCorrectWorkingDays(string[] dayName, TimeSpan[] startTime)
    {
        if (startTime.Count() != dayName.Count()) throw new Exception("Time is not defined to schedule");
        if (startTime.Count() > 6) throw new Exception("To many schedule days");
        var dayTimeDict = new Dictionary<string, TimeSpan>();
        for (int i = 0; i < dayName.Length; i++)
        {
            if (dayTimeDict.ContainsKey(dayName[i])) continue;
            dayTimeDict.Add(dayName[i], startTime[i]);
        }

        


        return true;
    }

    
    public async Task<Group> CreateScheduleDaysToGroup(Group item, string[] dayName, TimeSpan[] startTime)
    {
        try
        {
            await CheckCorrectWorkingDays(dayName, startTime);

            //var group = await _context.Groups.Include(x => x.DaySchedules).FirstOrDefaultAsync(x => x.Id == item.Id);
            if (item != null && item.DaySchedules != null)
            {

                var scheduleDaysInGroup = item.DaySchedules;

                _context.WorkingDays.RemoveRange(scheduleDaysInGroup);
                item.DaySchedules = new List<WorkingDay>();
                _context.Groups.Update(item);
                await _context.SaveChangesAsync();
                var item22 = new List<WorkingDay>();

                //item = group;
            }


            var lessonType = await _context.LessonTypes.FirstOrDefaultAsync(x => x.Id == item.LessonTypeId) ?? throw new Exception("Lesson type not found");
            var duration = lessonType.LessonTimeInMinutes;
            var schedules = new List<WorkingDay>();

            

            //Create workingDat = schedule with dayName, startTime and endTime
            for (int i = 0; i < dayName.Count(); i++)
            {
                var scheduleDay = new WorkingDay()
                {
                    DayName = Enum.Parse<DayOfWeek>(dayName[i]),
                    WorkingStartTime = startTime[i],
                    WorkingEndTime = startTime[i] + TimeSpan.FromMinutes(duration),
                    Group = item
                };

                var validations = new WorkingDayValidator();
                var validationResult = await validations.ValidateAsync(scheduleDay);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors;
                    var errorsString = string.Concat(errors);
                    throw new Exception(errorsString);
                }


                schedules.Add(scheduleDay);
            }
            await _context.DaySchedules.AddRangeAsync(schedules);
            item.DaySchedules = schedules;

            _context.Groups.Update(item);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex) { }

        return item;
    }
    #endregion

    public async Task<Group> EditAsync(Group item)
    {
        if (item == null) throw new Exception("Not correct data for group");
        if (item.IsLateDateStart) item.DateStart = DateTime.MinValue;

        #region CheckCorrect

        var userRegisterValidator = new GroupValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }
       
        if (_context.Groups.FirstOrDefault(x => x.Name == item.Name) != null) throw new Exception("Group with such name already exist");
       
        #endregion

        if(item.Lessons != null && item.Lessons.Count()!= null)
        {

        }
        _context.Groups.Update(item);
        await _context.SaveChangesAsync();


        return item;
    }
    

    public async Task<Group> UpdateGroupStudents(Group group, List<Guid> studentsId)
    {
        try
        {
            if (group != null && group.GroupStudents != null)
            {
                var groupStudents = group.GroupStudents.ToList();
                       
                foreach (var student in groupStudents)
                {
                    
                    
                    if (studentsId.Contains(student.Id))
                    {
                        _context.GroupStudents.Remove(student);

                        try
                        {
                            var usersToSend = new List<NotificationUser>() { new NotificationUser() { UserId = student.Student.ApplicationUser.Id } };
                            var message = " You was removed from group " + group.Name;
                            var notification = new NotificationMessage() { Message = message, Users = usersToSend };

                            usersToSend.ForEach(x => x.NotificationMessage = notification);
                            await _context.NotificationUsers.AddRangeAsync(usersToSend.AsEnumerable());
                            await _context.NotificationMessages.AddAsync(notification);
                            await _context.SaveChangesAsync();
                        }catch { }

                    }
                    try
                    {
                        if (!studentsId.Contains(student.Id))
                    {
                      
                            var usersToSend = new List<NotificationUser>() { new NotificationUser() { UserId = student.Student.ApplicationUser.Id } };
                            var message = " You was added to group " + group.Name;
                            var notification = new NotificationMessage() { Message = message, Users = usersToSend };

                            usersToSend.ForEach(x => x.NotificationMessage = notification);
                            await _context.NotificationUsers.AddRangeAsync(usersToSend.AsEnumerable());
                            await _context.NotificationMessages.AddAsync(notification);
                            await _context.SaveChangesAsync();
                        
                    }
                    }
                    catch { }

                    _context.Entry(student).State = EntityState.Detached;

                   
                   

                    /*
                    if (student2.Groups != null)
                    {
                        student2.Groups.Remove(group);
                        group.GroupStudents.Remove(student2);

                        _context.Students.Update(student2);
                        _context.Groups.Update(group);
                    }*/
                }
                //_context.Students.RemoveRange(groupStudents);
                await _context.SaveChangesAsync();
            }



            //add all
            foreach (var studentId in studentsId)
            {
                ApplicationUser? userr;

                var stud = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId);
                var groupNew = new Group() { Id = group.Id };//await _context.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == group.Id);

                if (await _context.GroupStudents.FirstOrDefaultAsync(x => x.GroupId == group.Id && x.StudentId == studentId) != null) continue;

                var grSt = new GroupStudent() { GroupId = group.Id, StudentId = studentId };
                /*
                if (stud.Groups != null) stud.Groups.Add(group);
                else  stud.Groups = new List<Group>() { group };

                //if (group != null && group.GroupStudents != null) group.GroupStudents.Add(stud);
                //else group.GroupStudents = new List<Student>() { stud };
                */
                await _context.GroupStudents.AddAsync(grSt);

                //_context.Entry(grSt.Group).State = EntityState.Unchanged;
                //_context.Entry(grSt.Student).State = EntityState.Unchanged;
                //await _context.SaveChangesAsync();
                /*
                _context.Entry(stud.Groups).State = EntityState.Unchanged;
                _context.Entry(group.GroupStudents).State = EntityState.Unchanged;

                _context.Students.Update(stud);
                _context.Groups.Update(group);
                */

                

            }

            
            await _context.SaveChangesAsync();
            }
            catch (Exception ex) { }
            return group;

    }

    public async Task<Group> HardDeleteAsync(Guid id)
    {
        var group = await GetAsync(id);
        var groupStudents = group.GroupStudents;
        var lessons = group.Lessons;
        var teacher = group.Teacher;

        foreach(var  i in lessons)
        {
            await _lessonService.DeleteLessonByGroup(group, i);
        }
        foreach(var i in groupStudents)
        {
            _context.Entry(i.Student).State = EntityState.Unchanged;
            _context.Entry(i.Group).State = EntityState.Unchanged;
            _context.GroupStudents.Remove(i);

        }
        if(teacher.Groups != null) 
        {
            teacher.Groups.Remove(new Group() { Id = id});
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();

            group.Teacher = null;
        }
        //_context.Entry(group.Teacher).State = EntityState.Unchanged;
        _context.Groups.Remove(group);
        await  _context.SaveChangesAsync();


        return group;
    }


    /*
     
    public async Task<List<Lesson>> CreateLessonBySchedule(Group group,List<WorkingDay> schedules)
    {

        var lessons = new List<Lesson>();
        DateTime date;

        if (group.DaySchedules == null || group.DaySchedules.Count == 0) throw new Exception("no schedule in group to create new day by schedule");
        if (group.Lessons != null)
            date = group.Lessons.OrderByDescending(x => x.StartTime).FirstOrDefault().StartTime;
        else
            date = group.DateStart;

        for (int lesCount = 0, scCount = 0; lesCount < schedules.Count; lesCount++, scCount++)
        {
            if (scCount == schedules.Count)
            {
                scCount = 0;
                date = date.AddDays(7);
            }
            var addingDays = LessonMath.Mod((int)date.DayOfWeek, (int)schedules[scCount].DayName);
            var virtualDate = date.AddDays(addingDays);
            var lesson = new Lesson()
            {
                StartTime = virtualDate + schedules[scCount].WorkingStartTime ?? DateTime.Now,
                EndTime = virtualDate + schedules[scCount].WorkingEndTime ?? DateTime.Now,
                //Group = group,
                //ArrivedStudents = lessonStudents,
                //Teacher = group.Teacher ?? new Teacher()
            };
            lessons.Add(lesson);
        }
        return lessons;


    }
    */








}
