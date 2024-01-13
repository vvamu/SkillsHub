using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services;
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

    public TeachersController(IUserService userService, 
        ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _userService = userService;
        _context = context;
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var parameters = new QueryStringParameters() { PageNumber = 1, PageSize = 100 };
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
            if (user == null) { user = teacher.ApplicationUser;
            }

            if (teacher == null)
            {
                //user = await _context.ApplicationUsers.FirstOrDefaultAsync(x=>x.Id == item.ApplicationUserId);
                teacher = await _userService.CreateTeacherAsync(user, item);
                item = teacher;
            }

            item.WorkingDays = String.Join("-", workingDay);
            _context.Teachers.Update(item);
            await _context.SaveChangesAsync();

            try
            {
                var ff = new TimeSpan();
                
                var res = await _userService.UpdateTeacherWithCourcesNames(item, itemValue.ToList());
                item = res;
            }catch(Exception ex) { }
            
            


            //user = await _userService.GetUserByIdAsync(item.ApplicationUserId);
            if (!User.Identity.IsAuthenticated)
            {
                user = await _userService.SignInAsync(user);
            }
        }catch (Exception ex) { Console.Write(ex.ToString()); }
        

        return RedirectToAction("Item", "Account", new { itemId = item.ApplicationUserId, id = item.ApplicationUserId });
    }

    [HttpGet]

    public async Task<IActionResult> Item(TeacherDTO item)
    {
        try
        {
            //var teacher = await _userService.GetUserByIdAsync(userId, item);
            //if (teacher.ApplicationUser.UserStudent != null) return RedirectToAction("Create", "Student");
            //if (await _userService.IsAdminAsync()) return RedirectToAction("Index", "CRM");
            //var userDb = await _userService.SignInAsync(item);
            //if (userDb == null) return View(userDb);
            //return RedirectToAction("Index", "CRM");
            return View(item);
        }
        catch (Exception ex) { return View(); }

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
        var items = _userService.GetAllTeachers().ToList().AsQueryable().Where(x=>x.IsDeleted == false);
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

        return Json(JsonSerializerToAjax.GetJsonByIQueriable(items));
    }

    


        public async Task<IActionResult> Activate(Guid id,Guid userId)
    {
        var currentUser = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == User.Identity.Name);
        if (id == Guid.Empty) RedirectToAction("Create", new { id = currentUser.Id });
        var item = await _context.Teachers.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.Id == id) ?? new Teacher() { ApplicationUserId = id};
        
        if (await _userManager.IsInRoleAsync(item.ApplicationUser, "Teacher"))
        {
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
