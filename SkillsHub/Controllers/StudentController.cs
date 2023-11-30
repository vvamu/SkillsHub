using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SkillsHub.Controllers;

public class StudentController : Controller
{
    private readonly IUserPresentationService _userPresentationService;
    private readonly ICourcesService _courcesService;
    private readonly IUserService _userService;

    public StudentController(IUserPresentationService userPresentationService, ICourcesService courcesService,
        IUserService userService)
    {
        _userPresentationService = userPresentationService;
        _courcesService = courcesService;
        _userService = userService;
        //var items = _userService.GetAllStudentsAsync().Result;
        //Reporter.ReportToXLS<StudentDTO>(items.ToArray());
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var parameters = new QueryStringParameters() { PageNumber = 1, PageSize = 100 };
        var items = await _userPresentationService.GetAllStudentsAsync(parameters);
        return View(items);
    }

    [HttpGet]
    [Route("/Student/GetStudentListAsync")]
    public async Task<IActionResult> GetStudentListAsync()
    {
        var items = await _userService.GetAllStudentsAsync();
        return Json(JsonSerializerToAjax.GetJsonByIQueriable(items));
    }

    [HttpGet]
    public IActionResult Edit()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Delete()
    {
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Create(Guid id)
    {
        //var user = await _userService.GetCurrentUserAsync() ?? throw new Exception("User not found");
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) user = await _userService.GetCurrentUserAsync();
            var student = new StudentDTO() { Id = user.Id };
            return View(student);
        }
        catch (Exception)
        {
            var a = HttpContext.Request.PathBase;
            var user = await _userService.GetCurrentUserAsync();

            if (user == null) return View("Index");
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudentDTO item, Guid[] itemValue)
    {
        var student = await _userService.CreateStudentAsync(item.Id, item);
        await _userService.CreatePossibleCourcesNamesToStudentAsync(student.Id, itemValue.ToList());

        if (await _userService.IsAdminAsync()) return RedirectToAction("Index", "CRM");

        var userDb = await _userService.SignInAsync(item);
        if (userDb == null) return View(userDb);
        return RedirectToAction("Index", "CRM");
    }
}
