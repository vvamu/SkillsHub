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
using SkillsHub.Helpers.SearchModels;
using SkillsHub.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace SkillsHub.Controllers;

[Authorize]
[Controller]
public class GroupController : Controller
{
    private readonly IGroupService _groupService;

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;
    private readonly ILessonService _lessonService;
    private readonly INotificationService _notificationService;

    public GroupController(IGroupService groupService , ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager, IUserService userService, ILessonService lessonService,
        INotificationService notificationService)
    {
        _groupService = groupService;
        _context = context;
        _userManager = userManager;
        _userService = userService;
        _lessonService = lessonService;
        _notificationService = notificationService;


    }
    public async Task<IActionResult> Index()
    {   
        var groups = await _groupService.GetAll().ToListAsync();
        return View(groups);
    }


    [HttpPost]
    public async Task<IActionResult> GetGroups(GroupFilterModel filters, OrderModel order)
    {
        var gr = _groupService.GetAllGroupsList();
        List<Group> res = new List<Group>();

        var ress = await FilterMaster.FilterGroups(gr.AsQueryable(), filters, order);
        var rerer = ress.ToList().Where(x=>!x.IsDeleted);
        


        foreach (var group in rerer)
        {

            var i = await _groupService.GetAsyncToGroupsList(group.Id);
            res.Add(i);

        }
        return PartialView("_GroupsList", res);
    }

    public async Task<IActionResult> Item(Guid id)
    {
        var group = await _groupService.GetAsync(id);
        if (group == null) return RedirectToAction("Index", "Group");
        /*
        if (!group.IsPermanentStaffGroup)
        {
            var gr = await _context.Groups.FindAsync(id);
            gr.IsPermanentStaffGroup = true;
            _context.Groups.Update(gr);
            await _context.SaveChangesAsync();
        }*/

        return View(group);
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid? id)
    {
        if(id == null) return View(new Group());

        var group = await _groupService.GetAsync((Guid)id);
        return View(group);
    }

    [HttpGet]
    [Route("/Group/ScheduleDays")]
    public async Task<IActionResult> GetScheduleDaysAsync(Guid groupId)
    {
        var item = await _groupService.GetAsync(groupId);
        if (item == null) return Ok(new List<GroupWorkingDay>());
        var scheduleDays = item.DaySchedules;
        scheduleDays?.ForEach(x => x.Group = null);

        return Ok(scheduleDays ?? new List<GroupWorkingDay>());
    }


    [HttpPost]
    [Route("/Group/Create2")]
    [AllowAnonymous]
    public async Task<IActionResult> Create2(Group group, Guid[] studentId, string[] dayName, TimeSpan[] startTime) //del teacherValue and duration
    {
       
        Group newGroup = new Group();
        if(group != null && group.Id != Guid.Empty) newGroup = await _context.Groups.FindAsync(group.Id);        

        try
        {

            group.TeacherId = new Guid(group?.TeacherIdString);
            if (!User.IsInRole("Admin")) group.IsVerified = false;
            else group.IsVerified = true;
            group.IsPermanentStaffGroup = true;

            if (newGroup.Id != Guid.Empty)
            {

                newGroup = await _groupService.UpdateAsync(group, studentId, dayName, startTime);
            }
            else
                newGroup =  await _groupService.CreateAsync(group, studentId,dayName,startTime);

        }
        catch (Exception ex)
        {
            if(ex is ArgumentNullException) ModelState.AddModelError("", "Группа не может быть создана без учителя");
            else ModelState.AddModelError("", ex.Message); 
            return View("Create", group);
        }
        return RedirectToAction("Item", new { id = newGroup.Id });
       
    }


    /*
    [HttpPost]
    public async Task<IActionResult> EditDateStart(Guid id,DateTime DateStart)
    {
        var group = await _groupService.GetAsync(id);


        if (DateStart < DateTime.Now) return RedirectToAction("Item", new { id = id }); ;
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
        return RedirectToAction("Item", new {id = id});


    }
    */
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var group = _context.Groups.FirstOrDefault(x => x.Id == id);
        group.IsDeleted = true; 
        _context.Groups.Update(group);
       await  _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [Authorize(Roles ="Admin")]
    [HttpPost]
    public async Task<IActionResult> HardDelete(Guid id)
    {
        try
        {


            await _groupService.DeleteAsync(id, true);
        }
        catch (Exception ex) {
            try
            {


                await _groupService.DeleteAsync(id, false);
            }
            catch (Exception ex2)
            {

            }
        }
        return RedirectToAction("Index");
    }
    /*
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

            
            foreach(var goodDay in goodDaysOfWeeksByStudent)
            {
                if (schedulesDays.Contains(goodDay))
                    goodGroups.Add(group);
            }
            
        }
        return goodGroups;
    }
    */

    


}
