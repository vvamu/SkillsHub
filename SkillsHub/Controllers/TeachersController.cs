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

    public TeachersController(IUserService userService, 
        ApplicationDbContext context, UserManager<ApplicationUser> userManager
        ,INotificationService notificationService)
    {
        _userService = userService;
        _context = context;
        _userManager = userManager;
        _notificationService = notificationService;
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

            var teacher = _context.Teachers.Include(x=>x.ApplicationUser).Include(x=>x.PossibleCources).FirstOrDefault(x=>x.ApplicationUser.Id==id) ?? new Teacher();
            var possibleCourses = await _context.Teachers.Where(x => x.ApplicationUserId == user.Id).SelectMany(x => x.PossibleCources).ToArrayAsync();
            ViewBag.PossibleCourses = possibleCourses;
            teacher.ApplicationUserId = id;
            return View(teacher);
        }
        catch (Exception ex) {
            var a = HttpContext.Request.PathBase;
            var user = await _userService.GetCurrentUserAsync();

            if(user == null) return View("Index");
            return View();

        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Teacher item, Guid[] itemValue, string[] workingDay)
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
            item.WorkingDays = String.Join("-", workingDay);
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
    public IActionResult Edit()
    {
        return View();
    }
    public IActionResult Delete()
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/Teachers/GetTeachersAsync")]
    public async Task<IActionResult> GetAllTeachers()
    {
        var items = _userService.GetAllTeachers().ToList().AsQueryable().Where(x=>x.IsDeleted == false).ToList();

        var res = new List<Teacher>();
        foreach(var i in items)
        {
            if (await _userManager.IsInRoleAsync(i.ApplicationUser, "Teacher"))
                res.Add(i);
        }
        

        try
        {
            var p = HttpContext.Request.QueryString.Value ?? "";
            p = p.Substring(1); // Remove '?'
            var queryString = System.Web.HttpUtility.UrlDecode(p);

            // Deserialize the JSON string
            var jsonObject = JObject.Parse(queryString);
            var filterModel = jsonObject["TeachersFilterModel"].ToObject<TeacherFilterModel>();
            var orderModel = jsonObject["TeachersOrderModel"].ToObject<TeacherOrderModel>();
            var a = "";

            //items = await FilterMaster.GetAllTeachers(items, filterModel, orderModel);

            // Convert the values to StudentFilterModel object
            /*
            var filterModel = new StudentFilterModel
            {
                ParentName = jsonObject.filters.ParentName ?? string.Empty,
                PossibleCourse = jsonObject.filters.PossibleCourse ?? string.Empty,
                MinDateCreated = jsonObject.filters.MinDateCreated ?? default(DateTime)
            };
            */

            //var result = ParseQueryString(p);


        }
        catch (Exception ex) { }

        return Json(JsonSerializerToAjax.GetJsonByIQueriable(res.AsQueryable()));
    }

    


        public async Task<IActionResult> Activate(Guid id,Guid userId)
    {
        var currentUser = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == User.Identity.Name);
        if (id == Guid.Empty) RedirectToAction("Create", new { id = currentUser.Id });
        var item = await _context.Teachers.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.Id == id) ?? new Teacher() { ApplicationUserId = id};
        
        if (await _userManager.IsInRoleAsync(item.ApplicationUser, "Teacher"))
        {
            item.IsDeleted = true;
            await _userManager.RemoveFromRoleAsync(item.ApplicationUser, "Teacher");
        }
        else
        {
            item.IsDeleted = false;
            await _userManager.AddToRoleAsync(item.ApplicationUser, "Teacher");
        }

        _context.Teachers.Update(item);
        await _context.SaveChangesAsync();
        return RedirectToAction("Item", "Account", new { itemId = item.ApplicationUserId, id = item.ApplicationUserId });

    }


}
