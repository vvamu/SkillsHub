using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;

namespace SkillsHub.Controllers;

public class GroupController : Controller
{
    private readonly IGroupService _groupService;

    private readonly ICourcesService _courcesService;


    public GroupController(IGroupService groupService,ICourcesService courcesService)
    {
        _groupService = groupService;
        _courcesService = courcesService;
    }
    public IActionResult Index()
    {
        ViewBag.CourceNames = _courcesService.GetAllCourcesNames();
        //ViewBag.Students = 
        return View(_groupService.GetAll());
    }

    [HttpPost]
    public void Create(Group group)
    {
        _groupService.CreateAsync(group);

    }

    public IActionResult GetCreateModal()
    {
        return PartialView("_Create");
    }

}
