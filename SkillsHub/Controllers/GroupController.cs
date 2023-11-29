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
    public async Task<IActionResult> Item(Guid id)
    {
        var group = await _groupService.GetAsync(id);
        return View(group);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Group item, Guid[] itemValue, Guid[] teacherValue)
    {
        /*
        List<Guid> arrivedStudentsId = new List<Guid>();
        for (int i = 0; i < itemValue.Length; i++)
        {
            if (itemValue[i])
                arrivedStudentsId.Add(itemId[i]);
        }
        */

        try
        {

            //string selected = Request.Form["itemValue"];
            item.TeacherId = teacherValue[0];

            var group = await _groupService.CreateAsync(item);
            await _groupService.AddStudentsToGroupAsync(group.Id, itemValue.ToList());
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message); return View();
        }
        return RedirectToAction("Index");
    }
    public IActionResult GetCreateModal()
    {
        return PartialView("_Create");
    }

}
