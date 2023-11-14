using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
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

    }
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

        var parameters = new QueryStringParameters() { PageNumber = 1, PageSize = 100 };
        var items = await _userService.GetAllStudentsAsync();
        string json = "";

        try
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            json = JsonSerializer.Serialize(items, options);
        }
        catch(Exception ex) { }
        return Json(json);
    }

    public IActionResult Create()
    {
        ViewBag.CourcesNames = _courcesService.GetAllCourcesNames();
        return View();
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
