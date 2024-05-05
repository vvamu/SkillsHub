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

	public LessonTypeController(ApplicationDbContext context)
    {
        _context = context;

	}


    #region GET

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var items = _context.LessonTypes.AsQueryable();
        return PartialView("Index", items);

    }


    [HttpPost]
    public async Task<IActionResult> OptionsList(LessonTypeFilterModel filters, OrderModel? order,string lessonTypeId)
    {
        var lessonTypes = _context.LessonTypes.AsAsyncQueryable();
        //lessonTypes = await FilterMaster.FilterLessonTypes(lessonTypes, filters, order);
        var idVal = Guid.TryParse(lessonTypeId?.ToString(), out var result) ? result : Guid.Empty;

        return PartialView((lessonTypes, idVal));
    }

    public async Task<IActionResult> Item(Guid id)
    {
        var item = await _context.Lessons.FirstOrDefaultAsync(x => x.Id == id);
        return View(item);
    }

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


    #endregion


    #region Create

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View("CreateLessonType");
    }

    [Route("/LessonType/CreateRowAsync")]
    [HttpGet]
    public async Task<IActionResult> CreateRowAsync()
    {
        return PartialView("Row", new LessonType() { Id = Guid.NewGuid() });
    }


    [HttpPost]
    public async Task<IActionResult> Create(LessonType item) //TODO
    {
        try
        {
            //await _courcesService.CreateLessonType(item);
            //_context.LessonTypes.Update(item);

        }
        catch (Exception ex)
        {
            try
            {
                //_context.LessonTypes.Add(item);
            }
            catch (Exception ex2)
            {
                ModelState.AddModelError("", ex2.Message);
                return View("CreateLessonType");
            }
           
        }
        return RedirectToAction("InstitutionSetting", "CRM");


    }

    #endregion

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


}
