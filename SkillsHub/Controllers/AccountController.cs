using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IUserService _userService;
    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllAsync();
        return View(users);
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Item(Guid itemId)
    {
        var currentUser = await _userService.GetCurrentUserAsync();
        if (itemId == Guid.Empty) itemId = currentUser.Id;

        ApplicationUser? user = await _userService.GetUserByIdAsync(itemId);

        return View(user);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDTO userCreateModel)
    {

        try
        {
            //if (!ModelState.IsValid) { ModelState.AddModelError("", ModelState.Values.ToString()); return View(); }
            var user = await _userService.CreateUserAsync(userCreateModel);
            if (userCreateModel.IsStudent) return RedirectToAction("Create", "Students");
            if (userCreateModel.IsTeacher) return RedirectToAction("Create", "Teachers");
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(); }


        return View();
    }



    [HttpGet]
    [AllowAnonymous]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(UserLoginDTO? user)
    {
        try
        {
            if (!ModelState.IsValid) { ModelState.AddModelError("", ModelState.Values.ToString()); return View(); }
            await _userService.InitialCreateAsync();
            var userDb = await _userService.SignInAsync(user);
            if (userDb == null) return View();
            return RedirectToAction("Index", "CRM");
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(); }

    }

     
}
