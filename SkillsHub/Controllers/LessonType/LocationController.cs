﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Helpers;
using SkillsHub.Persistence;

namespace SkillsHub.Controllers;

[Authorize]
public class LocationController : Controller
{
	private readonly ApplicationDbContext _context;
    private readonly AbstractLessonTypeLogModelService<Location>? _locationService;

    public LocationController(ApplicationDbContext context,IAbstractLogModel<Location> paymentCategoryService)
	{
		_context = context;
        _locationService = paymentCategoryService as LocationService;

    }
    public async Task<IActionResult> Index()
    {
        var items =  _locationService.GetAllItemsToList();
        var result = await items.Where(x => x.ParentId == null).ToListAsync();
        return PartialView( result);
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid? itemId)
    {
        var res = await _locationService.GetLastValueAsync(itemId);
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
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(item); }

        if (result == null) return RedirectToAction("InstitutionSetting", "CRM");
        return View(result);
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