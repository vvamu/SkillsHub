using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Helpers;
using SkillsHub.Persistence;

namespace SkillsHub.Controllers;
public class GroupTypeController : Controller
{
    private readonly AbstractLessonTypeLogModelService<GroupType>? _groupService;
    private readonly ApplicationDbContext _context;

	public GroupTypeController(ApplicationDbContext context, IAbstractLogModel<GroupType> groupService)
    {
        _groupService = groupService as GroupTypeService;
        _context = context;

    }


    public async Task<IActionResult> Index()
    {
        var items = _groupService.GetAllItemsToList();
        var result = await items.ToListAsync();
        return PartialView(result);
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid? itemId)
    {
        var res = await _groupService.GetLastValueAsync(itemId);
        return View(res);
    }
    [HttpPost]
    public async Task<IActionResult> Create(GroupType item)
    {
        GroupType? result;
        try
        {
            var isEdit = await _context.GroupTypes.FindAsync(item.Id);
            if (isEdit != null) result = await _groupService.UpdateAsync(item);
            else result = await _groupService.CreateAsync(item);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(item); }

        return View(result);
    }

    public async Task<IActionResult> Remove(GroupType item)
    {
        try
        {
            var res = await _groupService.RemoveAsync(item.Id);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(item); }
        return RedirectToAction("InstitutionSetting", "CRM");
    }

    [HttpPost]

    public async Task<IActionResult> OptionsList(string groupTypeId)
    {
        var items =  _context.GroupTypes.AsAsyncQueryable();
        var idVal = Guid.TryParse(groupTypeId?.ToString(), out var result) ? result : Guid.Empty;

        return PartialView((items, idVal));
    }

}