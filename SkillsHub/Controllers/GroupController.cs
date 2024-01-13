using Microsoft.AspNetCore.Authorization;
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

[Authorize]
public class GroupController : Controller
{
    private readonly IGroupService _groupService;

    private readonly ICourcesService _courcesService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;
    private readonly ILessonService _lessonService;
    private readonly INotificationService _notificationService;

    public GroupController(IGroupService groupService,ICourcesService courcesService, ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager, IUserService userService, ILessonService lessonService,
        INotificationService notificationService)
    {
        _groupService = groupService;
        _courcesService = courcesService;
        _context = context;
        _userManager = userManager;
        _userService = userService;
        _lessonService = lessonService;
        _notificationService = notificationService;


    }
    public async Task<IActionResult> Index()
    {
        
        ViewBag.CourseNames = _courcesService.GetAllCourcesNames();
        ViewBag.Students = await _userService.GetAllStudentsAsync();
        ViewBag.Teachers =  _userService.GetAllTeachers();

        
        var groups = await _groupService.GetAll().ToListAsync();
        
        
        
        foreach(var gr in _context.Groups)
        {
            
            var g = new Group() { Id = gr.Id, IsVerified = true };
            //if (gr.DateStart == DateTime.MinValue)
            //    g.DateStart = DateTime.Now;

            await _groupService.HardDeleteAsync(gr.Id);
            
            //_context.Groups.Update(g);
            //_context.SaveChangesAsync();
        }
        
       

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
            if(User.IsInRole("Teacher"))
            {
                item.IsVerified = false;
                
                
            }
            
            group =  await _groupService.CreateAsync(item);

            if (User.IsInRole("Teacher"))
            {
                var user = await _userService.GetCurrentUserAsync();
                var message = "Teacher " + user.FirstName + " " + user.LastName + " " + user.Surname + " send request to create new group '" + item.Name + "'. Check it.";
                var notification = new NotificationMessage() { IsRequest = true, Message = message };
            }

            await _groupService.CreateScheduleDaysToGroup(group, dayName, startTime, studentId);

            if(item.IsVerified)
            {
                if(!item.IsLateDateStart)
                {
                    await _groupService.CreateLessonsBySchedule(group.DaySchedules, item.DateStart, item.LessonsCount, item, true);

                }
                await _groupService.UpdateStudentsInGroup(group, studentId.ToList());
            }


            await _context.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message); return View("Create",item);
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
            await _context.SaveChangesAsync();

            if (group.LessonsCount != item.LessonsCount)
                _notificationService.СreateToUpdateCountLessonsInGroup(group, group.LessonsCount, item.LessonsCount, null);



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
            if (group.IsUnlimitedLessonsCount) lessonsCount = 10;
            if(item.IsVerified && !item.IsLateDateStart && lessonsCount > 0) 
            {
                var lessons = await _groupService.CreateLessonsBySchedule(group.DaySchedules, dateStart, lessonsCount, item, true);
                
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message); return RedirectToAction("Index");
        }
        return RedirectToAction("Item" , new {id = item.Id});
    }

    [HttpPost]
    public async Task EditDateStart(Guid id,DateTime DateStart)
    {
        var group = await _groupService.GetAsync(id);
        group.IsLateDateStart = false;
        group.DateStart = DateStart;
        _context.Groups.Update(group);
        await _context.SaveChangesAsync();

        var lessonsCount = group.LessonsCount;

        if (group.Lessons != null && group.Lessons.Count != 0)
        {
            DateStart = group.Lessons.OrderByDescending(x => x.EndTime).FirstOrDefault().EndTime.AddDays(1); //Where(x=>x.EndTime <  DateTime.Now)
            lessonsCount = group.LessonsCount - group.Lessons.Count();//.Where(x => x.EndTime < DateTime.Now).Count();
        }
        if (group.IsUnlimitedLessonsCount) lessonsCount = 10;
        if (group.IsVerified && !group.IsLateDateStart && lessonsCount > 0)
        {
            var lessons = await _groupService.CreateLessonsBySchedule(group.DaySchedules, DateStart, lessonsCount, group, true);

        }


    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
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
    [Authorize(Roles ="Admin")]

    [HttpPost]
    public async Task<IActionResult> HardDelete(Guid id)
    {
        await _groupService.HardDeleteAsync(id);
        /*
        var group = await _groupService.GetAsync(id);

        int a = 10;
        int b = 20;
        (a, b) = (b, a);

        int c = b - a;
        group.IsDeleted = true;

        _context.Groups.Update(group);
        await _context.SaveChangesAsync();*/

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
