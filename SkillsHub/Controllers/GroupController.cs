using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Helpers;
using SkillsHub.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace SkillsHub.Controllers;

public class GroupController : Controller
{
    private readonly IGroupService _groupService;

    private readonly ICourcesService _courcesService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;
    private readonly ILessonService _lessonService;

    public GroupController(IGroupService groupService,ICourcesService courcesService, ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager, IUserService userService, ILessonService lessonService)
    {
        _groupService = groupService;
        _courcesService = courcesService;
        _context = context;
        _userManager = userManager;
        _userService = userService;
        _lessonService = lessonService;


    }
    public async Task<IActionResult> Index()
    {
        
        ViewBag.CourseNames = _courcesService.GetAllCourcesNames();
        ViewBag.Students = await _userService.GetAllStudentsAsync();
        ViewBag.Teachers =  _userService.GetAllTeachers();
        var groups = await _groupService.GetAll().Where(x=>x.IsDeleted ==false).ToListAsync();

        return View(groups);
    }
    public IActionResult IndexByFilters(string? filterStr, Guid? filterCourseId)
    {
        ViewBag.CourseNames = _courcesService.GetAllCourcesNames();
        //ViewBag.Students = 
        //return View(_groupService.GetAllByFilter(filterStr, filterCourseId));
        return View();
    }
    public async Task<IActionResult> Item(Guid id)
    {
        var group = await _groupService.GetAsync(id);
        ViewBag.Students = await _userService.GetAllStudentsAsync();
        ViewBag.Teachers = _userService.GetAllTeachers();
        

        return View(group);
    }

    public IActionResult Create()
    {
        return View();
    }



    [HttpPost]
    public async Task<IActionResult> Create(Group item, Guid[] studentId, string[] dayName, TimeSpan[] startTime) //del teacherValue and duration
    {
        Group group = new Group();
        try
        {
            
            group =  await _groupService.CreateAsync(item); 

            await _groupService.CreateScheduleDaysToGroup(group, dayName, startTime, studentId);
            await _groupService.CreateLessonsBySchedule(group.DaySchedules, item.DateStart, item.LessonsCount, item);

            await _groupService.UpdateStudentsInGroup(group, studentId.ToList());
            
            await _context.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message); RedirectToAction("Index");
        }
        return RedirectToAction("Item", new { id = group.Id });

    }
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var group = await _groupService.GetAsync(id);
        try
        {
            if (group != null && group.GroupStudents != null)
            {
                ViewBag.studentsByGroup = group.GroupStudents.ToArray();
            }
            ViewBag.teachers = _context.Teachers.Include(x => x.ApplicationUser).ToList();


        }
        catch (Exception ex) { }
        return View(group);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Group item, Guid[] studentId, string[] dayName, TimeSpan[] startTime)
    {
        try
        {
            //need
            var group = await _groupService.GetAsync(item.Id);
            if (item.LessonTypeId == Guid.Empty) item.LessonTypeId = group.LessonTypeId;
            if(item.CourseNameId == Guid.Empty) item.CourseNameId = group.CourseNameId;
            _context.Groups.Update(item);

            //await _groupService.CreateScheduleDaysToGroup(item, dayName, startTime, studentId);
            
                await _groupService.UpdateStudentsInGroup(group, studentId.ToList());
                //await _groupService.UpdateStudentsInGroup2()

        
            await _context.SaveChangesAsync();
            
            var dateStart = item.DateStart;
            var lessonsCount = item.LessonsCount;

            if (group.Lessons != null && group.Lessons.Count !=  0)
            {
                dateStart = group.Lessons.OrderByDescending(x=>x.EndTime).FirstOrDefault().EndTime.AddDays(1); //Where(x=>x.EndTime <  DateTime.Now)
                lessonsCount = item.LessonsCount - group.Lessons.Count();//.Where(x => x.EndTime < DateTime.Now).Count();
            }
            var lessons = await _groupService.CreateLessonsBySchedule(group.DaySchedules, dateStart, lessonsCount, item);
            //await _groupService.SaveLessonsBySchedule(group, studentId.ToList(), lessons);




            /*

            //var lessons = new List<Lesson>();
            var date = DateTime.Now.Date;

            for (int lesCount = 0, scCount = 0; lesCount < item.LessonsCount; lesCount++, scCount++)
            {
                if (scCount == schedules.Count)
                {
                    scCount = 0;
                    date.AddDays(7);
                }
                var addingDays = LessonMath.Mod((int)date.DayOfWeek, (int)schedules[scCount].DayName);
                var virtualDate = date.AddDays(addingDays);

                var lesson = new Lesson()
                {
                    Creator = await _userManager.GetUserAsync(User),
                    StartTime = virtualDate + schedules[scCount].WorkingStartTime ?? DateTime.Now,
                    EndTime = virtualDate + schedules[scCount].WorkingEndTime ?? DateTime.Now,
                    //ArrivedStudents = lessonStudents,
                    //Teacher = _context.Gr
                    // ArrivedStudents = _context.Students.Include(x=>x.Groups).Where(x=>x.Groups.Select(x=>x))
                    //ArrivedStudents = _context.Groups.Include(x => x.GroupStudents).SelectMany(x => x.GroupStudents);
                };
                lessons.Add(lesson);
            }
            var prevLessons = _context.Lessons.Include(x=>x.Group).Where(x => x.Group.Id == item.Id);
            _context.RemoveRange(prevLessons);

            var daySchedules = _context.DaySchedules.Include(x=>x.Group).Where(x => x.Group.Id == item.Id);
            _context.RemoveRange(daySchedules);

            var prevStudents = _context.Students.Where(x => x.Groups.Select(x=>x.Id).Contains(item.Id));
            prevStudents.ToList().ForEach(x=>x.Groups.ToList().Remove(item));

            item.Lessons = lessons;
            item.DaySchedules = schedules;

            var group = await _groupService.EditAsync(item);

            await _context.SaveChangesAsync();
            */
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message); return RedirectToAction("Index");
        }
        return RedirectToAction("Item" , new {id = item.Id});
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var group = await _groupService.GetAsync(id);

        int a = 10;
        int b = 20;
        (a, b) = (b, a);

        int c = b - a;
        group.IsDeleted = true;

        _context.Groups.Update(group);
       await  _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    public IActionResult GetCreateModal()
    {
        return PartialView("_Create");
    }
    public async Task<IActionResult> GetAllGroups()
    {
        var items = _groupService.GetAll();
        return Json(JsonSerializerToAjax.GetJsonByIQueriable(items));
    }


    public async Task<List<Group>> GetGoodGroupsToStudent(Guid itemId)
    {
        var student = await _context.Students.Include(x => x.WorkingDays).FirstOrDefaultAsync(x=>x.Id== itemId) ?? throw new Exception("Student not found");
        //var goodDaysOfWeeksByStudent = student.WorkingDays.Contains(x=>x.);
        List<Group> goodGroups = new List<Group>();

        foreach(var group in _context.Groups.Include(x => x.DaySchedules).Where(x=>x.IsDeleted == false))
        {
            var schedulesDays = group.DaySchedules.Select(x=>x.DayName);

            /*
            foreach(var goodDay in goodDaysOfWeeksByStudent)
            {
                if (schedulesDays.Contains(goodDay))
                    goodGroups.Add(group);
            }
            */
        }
        return goodGroups;
    }

}
