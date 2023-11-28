using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Helpers;

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
        return View(_groupService.GetAll());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Group item, Guid[] itemValue)
    {
        try
        {
            var group = await _groupService.CreateAsync(item);
            await _groupService.AddStudentsToGroupAsync(group.Id, itemValue.ToList());
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message); return View();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("/Group/AddStudentsToGroupAsync")]
    public async Task<IActionResult> AddStudentsToGroupAsync(Guid groupId, Guid[] studentsId)
    {
        //try { }
        await _groupService.AddStudentsToGroupAsync(groupId, studentsId.ToList());
        return Json("ok");
    }

    [HttpPost]
    [Route("/Group/AddTeacherToGroupAsync")]
    public async Task<IActionResult> AddTeacherToGroupAsync(Guid groupId, Guid teacherId)
    {
        await _groupService.AddTeacherToGroupAsync(groupId, teacherId);
        return Json("ok");
    }

    [HttpGet]
    [Route("/Group/Free")]
    public IActionResult GetFree()
    {
        var items = _groupService.GetFree();
        return Json(JsonSerializerToAjax.GetJsonByIQueriable(items));
    }


}
