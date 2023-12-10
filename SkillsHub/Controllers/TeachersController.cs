using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Helpers;
using SkillsHub.Persistence;
using System.ComponentModel.DataAnnotations;

namespace SkillsHub.Controllers;

public class TeachersController : Controller
{
    IUserService _userService;
    IUserPresentationService _userPresentationService;
    private readonly ApplicationDbContext _context;

    public TeachersController(IUserService userService, IUserPresentationService userPresentationService,ApplicationDbContext context)
    {
        _userService = userService;
        _userPresentationService = userPresentationService;
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var parameters = new QueryStringParameters() { PageNumber = 1, PageSize = 100 };
        var teachers = _userPresentationService.GetAllTeachers(parameters);
        return View(teachers);
    }
    public async Task<IActionResult> IndexFilter(Guid? categoryId)
    {
        var parameters = new QueryStringParameters() { PageNumber = 1, PageSize = 100 };
        var teachers = _userPresentationService.GetAllTeachers(parameters);
        return View(teachers);
    }


    [HttpGet]
    public async Task<IActionResult> Create(Guid id)
    {
        //var user = await _userService.GetCurrentUserAsync() ?? throw new Exception("User not found");
        try
        {

        
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) user = await _userService.GetCurrentUserAsync();
            var teacher = new Teacher() { ApplicationUserId = user.Id };
            return View(teacher);
        }
        catch (Exception ex) {
            var a = HttpContext.Request.PathBase;
            var user = await _userService.GetCurrentUserAsync();

            if(user == null) return View("Index");
            return View();
            //HttpContext.Current.Request.Url.AbsolutePath
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Create(Teacher item, Guid[] itemValue)
    {
        var teacher = _context.Teachers.Include(x=>x.ApplicationUser).FirstOrDefault(x => x.ApplicationUser.Id == item.ApplicationUserId);
        var teacherId = teacher.Id;
        if(teacherId == Guid.Empty)
        {
            var teacher2 = await _userService.CreateTeacherAsync(item.ApplicationUserId, item);
            teacherId = teacher2.Id;
        }

        await _userService.CreatePossibleCourcesNamesToTeacherAsync(teacherId, itemValue.ToList());

        if (teacher.ApplicationUser.UserStudent != null) return RedirectToAction("Create", "Student"); //if user first time create account

        var user = await _userService.GetUserByIdAsync(item.ApplicationUserId);
        if (!User.Identity.IsAuthenticated)
        {
            var userDb = await _userService.SignInAsync(user);
        }  
        if (await _userService.IsAdminAsync())
        {
            //return RedirectToAction("Index", "Teachers"); //if admin create

            //return Redirect(Request.Headers["Referer"].ToString());
        }//if admin create
        var te = _context.Teachers.Include(x => x.ApplicationUser).FirstOrDefault(x => x.ApplicationUser.Id == item.ApplicationUserId);


        return RedirectToAction("Item","Account",(user.Id,user.Id));
            //View("Item","Account");


        //if (userDb == null) return View(userDb);
        //return RedirectToAction("Index", "CRM");
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
    [Route("/Teachers/GetTeachersByLessonTypeAsync")]
    public async Task<IActionResult> GetTeachersByLessonTypeAsync(Guid lessonTypeId)
    {
        var items = await _userService.GetTeachersByLessonTypeAsync(lessonTypeId);
        return Json(JsonSerializerToAjax.GetJsonByIQueriable(items));
    }

    [HttpGet]
    [Route("/Teachers/GetTeachersAsync")]
    public async Task<IActionResult> GetAllTeachers()
    {
        var items = _userService.GetAllTeachers();
        return Json(JsonSerializerToAjax.GetJsonByIQueriable(items));
    }

}
