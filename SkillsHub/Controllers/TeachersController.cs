using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Helpers;
using SkillsHub.Helpers.SearchModels;
using SkillsHub.Persistence;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SkillsHub.Controllers;

[Authorize]

public class TeachersController : Controller
{
    IUserService _userService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IUserRoleModelService<Teacher> _teacherService;
    private readonly IGroupService _groupService;

    public TeachersController(IUserService userService, 
        ApplicationDbContext context, UserManager<ApplicationUser> userManager
        ,INotificationService notificationService
        ,IUserRoleModelService<Teacher> teacherService
        ,IGroupService groupService)
    {
        _userService = userService;
        _context = context;
        _userManager = userManager;
        _notificationService = notificationService;
        _teacherService = teacherService;
        _groupService = groupService;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var teachers = _userService.GetAllTeachers();
        return View(teachers);
    }

    public async Task<Teacher> CreateScheduleDaysToTeacherWihoutSaving(Teacher item, string[] dayName, TimeSpan[] startTime, int[] duration)
    {

        var schedules = "";
        for (int i = 0; i < dayName.Count(); i++)
        {
            schedules = string.Join("-", dayName[i]);
        }

        item.WorkingDays = schedules;
        _context.Teachers.Update(item);

        return item;
    }

    /*

    [HttpGet]
    public async Task<IActionResult> Create(Guid id)
    {
        //При создании пользователя в данный метод мы передаем ИД пользователя и дальше его используем
        //При редактировании мы так же должны использовать данное ИД для ИД пользователя.
        try
        {
            HttpContext.Session.SetString("isTeacher","false");

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) throw new Exception("We can't create Teacher without User. User not found."); //if (user == null) user = await _userService.GetCurrentUserAsync();

            var possibleCourses = await _context.Teachers.Where(x => x.ApplicationUserId == user.Id).SelectMany(x => x.PossibleCources).ToListAsync();
            var teacher = _context.Teachers.Include(x => x.ApplicationUser).Include(x => x.PossibleCources).FirstOrDefault(x => x.ApplicationUser.Id == id) 
                ?? new Teacher() { ApplicationUserId = id , PossibleCources = possibleCourses};
            teacher.ApplicationUserId = id;
            
            return View(teacher ?? new Teacher() { ApplicationUserId = id});
        }
        catch (Exception ex) {
            var a = HttpContext.Request.PathBase;
            var user = await _userService.GetCurrentUserAsync();

            if(user == null) return View("Index");
            return View(new Teacher() { ApplicationUserId = id });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Teacher item, Guid[] lessonTypeId) //, string[] workingDay
    {
        try
        {
            var teacher = _context.Teachers.AsNoTracking().Include(x => x.ApplicationUser).FirstOrDefault(x => x.ApplicationUser.Id == item.ApplicationUserId);
            ApplicationUser user = await _userService.GetUserByIdAsync(item.ApplicationUserId);//= student.ApplicationUser  ?? new ;
            if (user == null) { user = teacher.ApplicationUser;}

            if (teacher == null)
            {
                teacher = await _userService.CreateTeacherAsync(user, item);
                item = teacher;
            }
            #region CheckPaidAmount

            if (!User.IsInRole("Admin"))
            {
                    var message = "User in teacher account " + User.Identity.Name + " tryed to change payment amout from ";
                    await _notificationService.Create(message, null);
                    throw new Exception("An attempt was made to fix a field that is only accessible to the administrator. The request has been sent to the appropriate place");
            }
            #endregion

            #region WorkingDays
            //item.WorkingDays = String.Join("-", workingDay);
            _context.Teachers.Update(item);
            await _context.SaveChangesAsync();
            #endregion

            #region CourceNames
            try
            {
                var ff = new TimeSpan();
                //var res = await _userService.UpdateTeacherWithCourcesNames(item, itemValue.ToList());
                //item = res;
            }
            catch (Exception ex) { }
            #endregion





            //user = await _userService.GetUserByIdAsync(item.ApplicationUserId);
            return RedirectToAction("Create", "BaseUserIngo", new { userId = user.Id });



   
        }catch (Exception ex) { Console.Write(ex.ToString()); }
        

        return RedirectToAction("Item", "Account", new { itemId = item.ApplicationUserId, id = item.ApplicationUserId });
    }
    */

    [HttpGet]
    public async Task<IActionResult> Create(Guid id, bool isEdit = false)
    {
        try
        {
            var user = await _context.ApplicationUsers.FindAsync(id) ?? await _userService.GetCurrentUserAsync();
            Teacher teacher = new Teacher();
            HttpContext.Session.SetInt32("isEdit", Convert.ToInt16(isEdit));
            var dbTeacher2 = await _context.Teachers.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id);

            if (user.UserTeacher == null && dbTeacher2 == null) { teacher = await _teacherService.CreateAsync(new Teacher() { ApplicationUserId = user.Id }, new Guid[0]); return View(teacher);}
             teacher = await _teacherService.GetAsync(dbTeacher2.Id);
            return View(teacher);
        }
        catch (Exception)
        {
            var user = await _userService.GetCurrentUserAsync();

            if (user == null) return View("Index");
            return View(new Teacher() { ApplicationUserId = id });


        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Teacher item, Guid[] lessonTypeId) // string[]? workingDay , itemValue = courceName
    {
        Teacher teacher = await _context.Teachers.FindAsync(item.Id);
        ApplicationUser user = await _context.ApplicationUsers.FindAsync(item.ApplicationUserId);
        try
        {
            if (teacher == null)
            {
                teacher = await _teacherService.CreateAsync(item, lessonTypeId);
            }
            else
            {
                teacher = await _teacherService.UpdateAsync(item, lessonTypeId);
            }
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(item); }

        /*
            if (!User.IsInRole("Admin"))
            {
                var message = "User in teacher account " + User.Identity.Name + " tried to change payment amount ";
                await _notificationService.Create(message,null);
                await _userService.SignOutAsync();
                throw new Exception("An attempt was made to fix a field that is only accessible to the administrator. The request has been sent to the appropriate place");

                //return RedirectToAction("Index", "Home");

            }
        */


        var isTeacher = HttpContext.Session.GetString("isTeacher");
        HttpContext.Session.Remove("isEdit");
        if (isTeacher == "true")
        {

            return RedirectToAction("Create", "Teachers", new { id = user.Id });
        }
        else if (HttpContext.Session.GetInt32("isEdit") == 0)
        {
            return RedirectToAction("Create", "BaseUserInfo", new { userId = user.Id });
        }
        else
        {
            return RedirectToAction("Item", "Account", new { userId = user.Id, id = user.Id });
        }
    }

    [Route("/Teachers/GetAsync")]
    public async Task<IActionResult> GetAsync(Guid? lessonTypeId, string? groupId = "", bool isTotal = false)
    {
        List<Teacher> rs = null;
        Guid groupID;
        Guid.TryParse(groupId, out groupID);
        if (!string.IsNullOrEmpty(groupId) && groupID != Guid.Empty)
        {
            var group = await _groupService.GetAsync((Guid)groupID);
            if (isTotal && group.GroupTeachers != null) rs = group.GroupTeachers?.Select(x => x.Teacher).ToList();
            if(!isTotal && group.GroupTeachers != null) rs = group.CurrentGroupTeachers.Select(x=>x.Teacher).ToList();
            
        }
        var items = await _teacherService.GetAllAsync();
        if (lessonTypeId != null && lessonTypeId != Guid.Empty)
        {
            var res = items.ToList();
            items = res.Where(x => x.PossibleCources != null &&  x.CurrentPossibleCourses != null && x.CurrentPossibleCourses.Select(x => x.LessonTypeId).ToList().Contains((Guid)lessonTypeId)).AsAsyncQueryable();
           
        }
        rs = await items.ToListAsync();
        if (rs == null)
        {
            rs =  new List<Teacher>();
        }
     
        return Ok(rs);
        //items = items.Where(x => !x.IsDeleted);

        
 
    }

}
