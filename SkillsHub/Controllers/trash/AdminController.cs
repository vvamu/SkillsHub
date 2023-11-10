using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Services.Interfaces;

namespace SkillsHub.Controllers;

public class AdminController : Controller
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService) 
    {
        _userService = userService;
    }
    public IActionResult Index()
    {
        return View();
    }

    //public async Task<IActionResult> CreateTeacher(Guid userId,TeacherDTO item)
    //{

    //    var itemDb = await _userService.CreateTeacherAsync(userId,item);
    //    if(itemDb == null) return View(item);

    //    return RedirectToAction("Index");
    //}

    //[HttpGet]
    //public IActionResult CreateStudent() 
    //{
    //    return View();
    //}

    //[HttpPost]
    //public IActionResult CreateStudent(StudentDTO user)
    //{
    //    return View();
    //}

    //[HttpPost]
    //public IActionResult CreateUser(UserDTO user)
    //{
    //    return View();
    //}

}
