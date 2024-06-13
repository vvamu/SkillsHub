using AutoMapper;
using EmailProvider.Interfaces;
using EmailProvider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Helpers;
using SkillsHub.Models;
using SkillsHub.Persistence;
using System.Diagnostics;
using System.Text;
using static Viber.Bot.NetCore.Models.ViberResponse;

namespace SkillsHub.Controllers;
public class AgeTypeController : Controller
{
	private readonly ApplicationDbContext _context;
    private readonly AbstractLessonTypeLogModelService<AgeType>? _ageTypeService;

    public AgeTypeController(ApplicationDbContext context, IAbstractLogModel<AgeType> ageTypeService)
    {
        _context = context;
        _ageTypeService = ageTypeService as AgeTypeService;


    }

    public async Task<IActionResult> Index()
    {
        var items =  _ageTypeService.GetAllItemsToList();
        var result = await items.Where(x => x.ParentId == null).ToListAsync();
        return PartialView( result);
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid? itemId)
    {
        var res = await _ageTypeService.GetLastValueAsync(itemId);
        return View(res);
    }
    [HttpPost]
    public async Task<IActionResult> Create(AgeType item)
    {
        AgeType? result;
        try
        {
            var isEdit = await _context.AgeTypes.FindAsync(item.Id);
            if (isEdit != null)
            {
                var res = isEdit == item;
                result = await _ageTypeService.UpdateAsync(item);
            }
            else result = await _ageTypeService.CreateAsync(item);
        }catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(item);}
        
        return View(result);
        //return RedirectToAction("InstitutionSetting", "CRM");
    }

    public async Task<IActionResult> Remove(AgeType item)
    {
        try
        {
        var res = await _ageTypeService.RemoveAsync(item.Id);
        }catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(item);}
        return RedirectToAction("InstitutionSetting", "CRM");
    }

    /*
    [HttpPost]
    public async Task<IActionResult> OptionsList(string ageTypeId)
    {
        var items =  _context.AgeTypes.AsAsyncQueryable();
        var idVal = Guid.TryParse(ageTypeId?.ToString(), out var result) ? result : Guid.Empty;

        return PartialView((items, idVal));
    }
    */
}