using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Helpers;
using SkillsHub.Persistence;

namespace SkillsHub.Controllers;

public class GroupController : Controller
{
    private readonly IGroupService _groupService;

    private readonly ICourcesService _courcesService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public GroupController(IGroupService groupService,ICourcesService courcesService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _groupService = groupService;
        _courcesService = courcesService;
        _context = context;
        _userManager = userManager;
    }
    public IActionResult Index()
    {
        ViewBag.CourceNames = _courcesService.GetAllCourcesNames();
        //ViewBag.Students = 
        return View(_groupService.GetAll());
    }
    public IActionResult IndexByFilters(string? filterStr, Guid? filterCourseId)
    {
        ViewBag.CourceNames = _courcesService.GetAllCourcesNames();
        //ViewBag.Students = 
        return View(_groupService.GetAllByFilter(filterStr, filterCourseId));
    }
    public async Task<IActionResult> Item(Guid id)
    {
        var group = await _groupService.GetAsync(id);
        return View(group);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Group item, Guid[] itemValue, Guid[] teacherValue, string[] dayName, TimeSpan[] startTime, int[] duration)
    {
        try
        {
            item.TeacherId = teacherValue[0];

            var schedules = new List<UserDaySchedule>();
            for (int i = 0; i < dayName.Count(); i++)
            {
                var scheduleDay = new UserDaySchedule()
                {
                    DayName = Enum.Parse<DayOfWeek>(dayName[i]),
                    WorkingStartTime = startTime[i],
                    WorkingEndTime = startTime[i] + TimeSpan.FromMinutes(duration[i]),
                };
                schedules.Add(scheduleDay);
            }

            var lessons = new List<Lesson>();
            var date = DateTime.Now.Date;
            for (int lesCount = 0, scCount = 0; lesCount < item.LessonsCount; lesCount++, scCount++)
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
                    Creator = await _userManager.GetUserAsync(User),
                    StartTime = virtualDate + schedules[scCount].WorkingStartTime,
                    EndTime = virtualDate + schedules[scCount].WorkingEndTime,
                };
                lessons.Add(lesson);
            }

            item.Lessons = lessons;
            item.DaySchedules = schedules;
            var group = await _groupService.CreateAsync(item);
            await _groupService.AddStudentsToGroupAsync(group.Id, itemValue.ToList());

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message); return View();
        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        var group = await _groupService.GetAsync(id);
        return View(group);
    }

    [HttpPut]
    public async Task<IActionResult> Edit(Group item, Guid[] itemValue, Guid[] teacherValue, string[] dayName, TimeSpan[] startTime, int[] duration)
    {
        try
        {
            item.TeacherId = teacherValue[0];

            var schedules = new List<UserDaySchedule>();
            for (int i = 0; i < dayName.Count(); i++)
            {
                var scheduleDay = new UserDaySchedule()
                {
                    DayName = Enum.Parse<DayOfWeek>(dayName[i]),
                    WorkingStartTime = startTime[i],
                    WorkingEndTime = startTime[i] + TimeSpan.FromMinutes(duration[i]),
                };
                schedules.Add(scheduleDay);
            }

            var lessons = new List<Lesson>();
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
                    StartTime = virtualDate + schedules[scCount].WorkingStartTime,
                    EndTime = virtualDate + schedules[scCount].WorkingEndTime,
                };
                lessons.Add(lesson);
            }
            var prevLessons = _context.Lessons.Where(x => x.Group.Id == item.Id);
            _context.RemoveRange(prevLessons);
            var daySchedules = _context.DaySchedules.Where(x => x.Group.Id == item.Id);
            _context.RemoveRange(prevLessons);
            var prevStudents = _context.Students.Where(x => x.Groups.Select(x=>x.Id).Contains(item.Id));
            prevStudents.ToList().ForEach(x=>x.Groups.ToList().Remove(item));

            item.Lessons = lessons;
            item.DaySchedules = schedules;
            var group = await _groupService.EditAsync(item);
            await _groupService.AddStudentsToGroupAsync(group.Id, itemValue.ToList());

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message); return View();
        }
        return RedirectToAction("Index");
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var group = await _groupService.GetAsync(id);
        return View(group);
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

}
