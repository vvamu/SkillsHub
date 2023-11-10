using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SkillsHub.Controllers;

//[Route("CRM/Students")]
public class StudentsController : Controller
{
    private readonly IUserPresentationService _userPresentationService;
    private readonly ICourcesService _courcesService;

    public StudentsController(IUserPresentationService userPresentationService, ICourcesService courcesService)
    {
        _userPresentationService = userPresentationService;
        _courcesService = courcesService;
    }
    public async Task<IActionResult> Index()
    {
        var parameters = new QueryStringParameters() { PageNumber = 1, PageSize = 100 };
        var items = await _userPresentationService.GetAllStudentsAsync(parameters);
        return View(items);
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
