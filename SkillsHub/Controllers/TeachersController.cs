using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Interfaces;
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
    public async Task<IActionResult> Create(Guid userId)
    {
        //var user = await _userService.GetCurrentUserAsync() ?? throw new Exception("User not found");
        var user = await _userService.GetUserByIdAsync(userId);
        var teacher = new TeacherDTO() { UserId = user.Id };
        return View(teacher);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid userId,TeacherDTO item)
    {
        var teacher = await _userService.CreateTeacherAsync(userId,item);
        if (teacher.ApplicationUser.UserStudent != null) return RedirectToAction("Create", "Student");
        if (await _userService.IsAdminAsync()) return RedirectToAction("Index","CRM");
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

}
