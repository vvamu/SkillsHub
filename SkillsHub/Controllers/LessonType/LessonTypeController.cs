using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Helpers;
using SkillsHub.Helpers.SearchModels;
using SkillsHub.Persistence;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SkillsHub.Controllers;

[Authorize]

public class LessonTypeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILessonTypeService _lessonTypeService;

    public LessonTypeController(ApplicationDbContext context, ILessonTypeService lessonTypeService)
    {
        _context = context;
        _lessonTypeService = lessonTypeService;


    }


    #region GET

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var items =  _lessonTypeService.GetAll().Where(x=>x.ParentId == null || x.ParentId == Guid.Empty);
        var result = await items.ToListAsync();
        return PartialView( result);

    }


    [HttpGet]
    public async Task<IActionResult> OptionsList(LessonTypeFilterModel filters, OrderModel? order) // ,string lessonTypeId
    {
        var lessonTypes = await  _lessonTypeService.GetAll().Where(x=>x.ParentId == null && !x.IsDeleted).ToListAsync();
        //lessonTypes = await FilterMaster.FilterLessonTypes(lessonTypes, filters, order);
        //var idVal = Guid.TryParse(lessonTypeId?.ToString(), out var result) ? result : Guid.Empty;

        return Ok(lessonTypes);//PartialView((lessonTypes.AsAsyncQueryable(), idVal));
    }

    [HttpGet]
    [Route("/LessonType/CheckboxList")]
    public async Task<IActionResult> CheckboxList() // string lessonTypeId
    {
        var lessonTypes = await _lessonTypeService.GetAll().Where(x => !x.IsDeleted && x.ParentId == null).ToListAsync(); // && x.IsActive
        return Ok(lessonTypes);
    }

    public async Task<IActionResult> Item(Guid id)
    {
        var item =  _lessonTypeService.GetAll();
        return View(await item.FirstOrDefaultAsync(x => x.Id == id));
    }

    /*
    [HttpGet]
    [Route("/LessonType/GetAsync")]
    public async Task<IActionResult> GetAsync()
    {

        var items = await _context.LessonTypes.ToListAsync();
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
        catch (Exception ex) { }
        return Json(json);
    }
    [HttpGet]
    [Route("/Cource/GetLessonTypesAsync")]
    public async Task<IActionResult> GetLessonTypesAsync()
    {

        var items = await _context.LessonTypes.ToListAsync();
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
        catch (Exception ex) { }
        return Json(json);
    }
    */

    #endregion


    #region Create

    [HttpGet]
    public async Task<IActionResult> Create(Guid? itemId)
    {
        LessonType model = await _lessonTypeService.GetLastValueAsync(itemId);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(LessonType item, Guid[]? paymentCategories) 
    {
        LessonType result = null;
        try
        {
            var isEdit = await _context.LessonTypes.FindAsync(item.Id);
            if(isEdit == null) result = await _lessonTypeService.CreateAsync(item,paymentCategories);   
            else result = await _lessonTypeService.UpdateAsync(item,paymentCategories);

        }
        catch (Exception ex)
        {
           ModelState.AddModelError("", ex.Message); return View("Create",item);
        }
        if(result == null) return RedirectToAction("InstitutionSetting", "CRM");
        return View(result);


    }

    #endregion

    public async Task<IActionResult> Remove(LessonType item)
    {
        LessonType lessonType;
        try
        {
            if(!item.IsDeleted) lessonType = await _lessonTypeService.RemoveAsync(item.Id);
            else lessonType = await _lessonTypeService.RestoreAsync(item.Id);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View("Create",item); }
        return RedirectToAction("InstitutionSetting", "CRM");
    }

    #region trash

    /*
    [Route("/LessonType/DeleteAsync")]
    [HttpPost]
    public IActionResult DeleteAsync(Guid itemId)
    {
        return Json(new { success = true });
    }


    public IActionResult SoftDeleteLessonType(LessonType item)
	{
		var courceName = _context.LessonTypes.FirstOrDefault(c => c.Id == item.Id);
		if (courceName == null) { return RedirectToAction("Index"); }
		courceName.IsDeleted = true;
		_context.LessonTypes.Update(courceName);
		_context.SaveChanges();
		return RedirectToAction("Index");
	}

    public IActionResult HardDeleteLessonType(LessonType item)
    {
        var courceName = _context.LessonTypes.FirstOrDefault(c => c.Id == item.Id);
        if (courceName == null) { return RedirectToAction("Index"); }
	
        courceName.IsDeleted = true;
        _context.LessonTypes.Update(courceName);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
	    [HttpPost]
    public async Task SaveLessonType(LessonType item)
    {
        if (item.Id == Guid.Empty) return;

        var newItemId =  Guid.NewGuid();
        item.Parent = new LessonType() { Id = newItemId };
        item.IsDeleted = true;
        _context.LessonTypes.Update(item);
        await _context.SaveChangesAsync();

        var newItem = item;
        newItem.Parent = null;
        newItem.IsDeleted = false;
        newItem.Id = newItemId;
        await _context.LessonTypes.AddAsync(newItem);
        await _context.SaveChangesAsync();

        await _context.SaveChangesAsync();
    }

    public IActionResult DeleteLessonType(LessonType item)
    {
        var courceName = _context.LessonTypes.FirstOrDefault(c => c.Id == item.Id);
        if (courceName == null) { return RedirectToAction("Index"); }
        courceName.IsDeleted = true;
        _context.LessonTypes.Update(courceName);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    */

    #endregion

}
