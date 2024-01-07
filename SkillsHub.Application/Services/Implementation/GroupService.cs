using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using SkillsHub.Persistence.Migrations;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Group = SkillsHub.Domain.Models.Group;

namespace SkillsHub.Application.Services.Implementation;

public class GroupService: IGroupService
{
    private readonly ApplicationDbContext _context;

    public GroupService(ApplicationDbContext context)
    {
        _context = context;
    }
    

    #region Get
    public IQueryable<Group> GetAll() => _context.Groups
        .Include(x => x.Lessons).Include(x => x.CourseName).Include(x => x.LessonType)
        .Include(x => x.GroupStudents).ThenInclude(x=>x.Student).ThenInclude(x => x.ApplicationUser)
        //.Include(x => x.GroupStudents).ThenInclude(x => x.Student).ThenInclude(x=>x.L)
        .Include(x => x.DaySchedules)
        .Include(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
        .Include(x => x.CourseName)
        .Include(x => x.LessonType)
        .AsQueryable();
    public async Task<Group> GetAsync(Guid id)
    {
        var groups = await _context.Groups
            .Include(x => x.Lessons).Include(x => x.CourseName).Include(x => x.LessonType)
            //.Include(x => x.GroupStudents).ThenInclude(x => x.Group)
            .Include(x => x.GroupStudents).ThenInclude(x => x.Student).ThenInclude(x=>x.ApplicationUser)
            .Include(x => x.DaySchedules)
            .Include(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.CourseName)
            .Include(x => x.LessonType).ToListAsync();
        var item = groups.Find(x => x.Id == id);
        return item;
    }

    public IQueryable<Group> GetAllByFilter(string? filterStr, Guid? filterCourseId)
    {
        var groups = _context.Groups
            .Include(x => x.Lessons).Include(x => x.CourseName).Include(x => x.LessonType)
            .Include(x => x.GroupStudents).ThenInclude(x => x.Student).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.DaySchedules)
            .Include(x => x.Teacher).ThenInclude(x => x.ApplicationUser).AsQueryable();

        if (filterCourseId != Guid.Empty) groups = groups.Where(x => x.CourseNameId == filterCourseId);
        if (!string.IsNullOrEmpty(filterStr)) groups = groups.Where(x => x.Name.Contains(filterStr));
        //сортировка по времени создания
        return groups;
    }

    #endregion

    #region Create
    public async Task<Group> CreateAsync(Group item)
    {
        if (item == null) throw new Exception("Not correct data for group");
        var userRegisterValidator = new GroupValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }
        var cource = await _context.CourseNames.FirstOrDefaultAsync(x => x.Id == item.CourseNameId) ?? throw new Exception("Cource not found");
        var lessonType = await _context.LessonTypes.FirstOrDefaultAsync(x => x.Id == item.LessonTypeId) ?? throw new Exception("LessonType not found");
        if (_context.Groups.FirstOrDefault(x => x.Name == item.Name) != null) throw new Exception("Group with such name already exist");

        var teacher = _context.Teachers.FirstOrDefault(x => x.Id == item.TeacherId);
        ApplicationUser? userr;
        if (teacher == null)
        {
            userr = await _context.ApplicationUsers.Include(x => x.UserTeacher).FirstOrDefaultAsync(x => x.Id == item.TeacherId);
            if (userr != null)
                teacher = userr.UserTeacher;
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

    public async Task<List<Lesson>> CreateLessonsBySchedule(List<WorkingDay> schedules, DateTime startDate, int countLessons, Group group)
    {
        var lessons = new List<Lesson>();
        var groupLessons = _context.Lessons.Include(x => x.Group).Where(x => x.Group.Id == group.Id);
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
                    Group = group, ///
                    //ArrivedStudents = lessonStudents,
                    //Teacher = group.Teacher ?? new Teacher()
                };
                lessons.Add(lesson);
                _context.Entry(lesson.Group).State = EntityState.Unchanged;



                if (groupLessons != null && !groupLessons.Any(x => x.StartTime == lesson.StartTime && x.EndTime == lesson.EndTime))
                {
                    _context.Lessons.Add(lesson);

                }

            }
            ///
            group.Lessons = lessons;
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();


            //await AddStudentsToLessons(group, group.GroupStudents.Select(x => x.Id).ToList(), lessons);

        }



        catch (Exception ex) { }

        return lessons;


    }
    

    public async Task<Group> CreateScheduleDaysToGroup(Group item, string[] dayName, TimeSpan[] startTime, Guid[] studentIds)
    {
        try
        {
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

            if (startTime.Count() != dayName.Count()) throw new Exception("Time is not defined to schedule");

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

    public async Task<Group> UpdateLessonsInGroup(Group item, List<Guid> studentIds)
    {
        var lessons = item.Lessons ?? await _context.Lessons.Include(x => x.Group).Where(x => x.Group.Id == item.Id).ToListAsync();
        //if (lessons == null) return null;

        //Create lessons students
        try
        {
            var groupStudents = item.GroupStudents ?? new List<GroupStudent>();

            var teacher = item.Teacher ?? await _context.Teachers.Include(x => x.Groups).FirstOrDefaultAsync(x => x.Groups.Select(x => x.Id).Contains(item.Id));

            //await CreateLessonStudents(lessons, studentIds, teacher);

            foreach (var lesson in lessons)
            {
                lesson.Group = item;
                lesson.Teacher = teacher;

            }

            item.Lessons = lessons;
            item.GroupStudents = groupStudents;

            _context.Lessons.UpdateRange(lessons);
            _context.Groups.Update(item);
            //_context.Teachers.Update(item.Teacher);





            await _context.SaveChangesAsync();

        }
        catch (Exception ex) { }

        return item;
    }

    public async Task<Group> EditAsync(Group item)
    {
        if (item == null) throw new Exception("Not correct data for group");
        var userRegisterValidator = new GroupValidator();
        var validationResult = await userRegisterValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }
        var cource = await _context.CourseNames.FirstOrDefaultAsync(x => x.Id == item.CourseNameId);
        var lessonType = await _context.LessonTypes.FirstOrDefaultAsync(x => x.Id == item.LessonTypeId);
        item.LessonType = lessonType;
        if (_context.Groups.FirstOrDefault(x => x.Name == item.Name) != null) throw new Exception("Group with such name already exist");
        if (cource == null) throw new Exception("Cource not found");
        if (lessonType == null) throw new Exception("LessonType not found");
        item.LessonType = lessonType;
        item.CourseName = cource;

        //var groupDb = new Group() { Name = item.Name, CourseName = cource , LessonType = lessonType };
        _context.Groups.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }
    

    public async Task<Group> UpdateStudentsInGroup(Group group, List<Guid> studentsId)
    {
        try
        {
            //remove all
            //var item = await _context.Groups.Include(x => x.DaySchedules).FirstOrDefaultAsync(x => x.Id == group.Id);
            var item = group;

            if (item != null && item.GroupStudents != null)
            {
                var groupStudents = item.GroupStudents.ToList();
                
                foreach (var student in groupStudents)
                {

                    //var student2 = _context.Students.Include(x => x.Groups).FirstOrDefault(x => x.Id == student.Id);
                    //var grSt = await _context.GroupStudents.Include(x=>x.Group).Include(x=>x.Student).FirstOrDefaultAsync(x => x.Group.Id == group.Id && x.Student.Id == student.Id);
                    //if (grSt == null) continue;

                    //_context.Entry(student.Group).State = EntityState.Unchanged;
                    //_context.Entry(grSt.Student).State = EntityState.Unchanged;
                    _context.GroupStudents.Remove(student);


                    /*
                    if (student2.Groups != null)
                    {
                        student2.Groups.Remove(item);
                        item.GroupStudents.Remove(student2);

                        _context.Students.Update(student2);
                        _context.Groups.Update(item);
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

                var grSt = new GroupStudent() { Group = groupNew, Student = stud };
                /*
                if (stud.Groups != null) stud.Groups.Add(item);
                else  stud.Groups = new List<Group>() { item };

                if (group != null && group.GroupStudents != null) group.GroupStudents.Add(stud);
                else group.GroupStudents = new List<Student>() { stud };
                */
                await _context.GroupStudents.AddAsync(grSt);

                _context.Entry(grSt.Group).State = EntityState.Unchanged;
                _context.Entry(grSt.Student).State = EntityState.Unchanged;
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
                //Group = item,
                //ArrivedStudents = lessonStudents,
                //Teacher = item.Teacher ?? new Teacher()
            };
            lessons.Add(lesson);
        }
        return lessons;


    }
    */

    






}
