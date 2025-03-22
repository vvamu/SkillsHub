using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Persistence;

namespace SkillsHub.Controllers;

[Authorize]
public class LocationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly AbstractLessonTypeLogModelService<Location>? _locationService;

    public LocationController(ApplicationDbContext context, IAbstractLogModelService<Location> paymentCategoryService)
    {
        _context = context;
        _locationService = paymentCategoryService as LocationService;

    }
    public async Task<IActionResult> Index()
    {
        var items = _locationService.GetItems(onlyCurrent: true);
        var result = await items.Where(x => x.ParentId == null).ToListAsync();
        return PartialView(result);
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid? itemId)
    {
        var res = await _locationService.GetLastValueAsync(itemId,withParents:true);
        return View(res);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Location item)
    {
        Location? result = null;
        try
        {

            var isEdit = await _context.Locations.FindAsync(item.Id);
            if (isEdit != null) result = await _locationService.UpdateAsync(item);
            else result = await _locationService.CreateAsync(item);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); var res = await _locationService.GetLastValueAsync(item.Id, withParents: true); return View(res); }

        if (result == null) return RedirectToAction("InstitutionSetting", "CRM");
        return RedirectToAction("Create", new { itemId = result.Id}); ;
    }
    public async Task<IActionResult> Remove(Location item)
    {
        try
        {
            var res = await _locationService.RemoveAsync(item.Id);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(item); }
        return RedirectToAction("InstitutionSetting", "CRM");
    }
    /*
    [HttpPost]
    public async Task<IActionResult> OptionsList(string paymentCategoryId)
    {
		var paymentCategories = _context.Locations.AsAsyncQueryable();
        var idVal = Guid.TryParse(paymentCategoryId?.ToString(), out var result) ? result : Guid.Empty;

        return PartialView((paymentCategories, idVal));
    }*/

}
