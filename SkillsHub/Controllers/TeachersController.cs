using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SkillsHub.Controllers;

public class TeachersController : Controller
{
    IUserService _userService;
    IUserPresentationService _userPresentationService;
    public TeachersController(IUserService userService, IUserPresentationService userPresentationService)
    {
        _userService = userService;
        _userPresentationService = userPresentationService;
    }
    public async Task<IActionResult> Index()
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
            var teacher = new TeacherDTO() { UserId = user.Id };
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
    public async Task<IActionResult> Create(TeacherDTO item, Guid[] itemValue)
    {

        var teacher = await _userService.CreateTeacherAsync(item.Id,item);
        await _userService.CreatePossibleCourcesNamesToTeacherAsync(teacher.Id, itemValue.ToList());

        if (teacher.ApplicationUser.UserStudent != null) return RedirectToAction("Create", "Student");
        if (await _userService.IsAdminAsync()) return RedirectToAction("Index","Teacher");

        var userDb = await _userService.SignInAsync(item);
        if (userDb == null) return View(userDb);
        return RedirectToAction("Index", "CRM");
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
