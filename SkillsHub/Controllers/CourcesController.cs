﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Persistence;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SkillsHub.Controllers;

public class CourcesController : Controller
{
    private readonly ApplicationDbContext _context;

    public CourcesController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var cources = _context.Cources.AsQueryable();
        var courcesNames = _context.CourceNames.AsQueryable();
        var lessonTypes = _context.LessonTypes.AsQueryable();
        return View((cources,courcesNames,lessonTypes));
    }

    [HttpPost]
    public async Task SaveLessonType(LessonType item)
    {
        _context.Update(item);
        await _context.SaveChangesAsync();
        
        //return Json(details.Count, JsonRequestBehavior.AllowGet);
    }

	[HttpPost]
	public async Task SaveCourcesNames(Cource item)
	{
		_context.Update(item);
		await _context.SaveChangesAsync();
	}

	[HttpPost]
	public async Task SaveLessonTypes(LessonType item)
	{
		_context.Update(item);
		await _context.SaveChangesAsync();
	}


	public async Task<IActionResult> Item(Guid id)
    {
        var item = await _context.Lessons.FirstOrDefaultAsync(x=> x.Id == id);
        return View(item);
    }

    public async Task GetCourcesNames(int sport) //JSON
    {
        //ViewBag.CourcesNames = _courcesService.GetAllCourcesNames();
        //ViewBag.EnglishLevels = _courcesService.GetAllEnglishLevels();
        //ViewBag.LessonTypes = _courcesService.GetAllLessonType();
        var arr = new String[] { "s", "f" };
        //var result = JsonArray.Create(arr);
        //return result;

        //User user = await _userManager.FindByNameAsync(User.Identity.Name);
        //var sports1 = db.Sports.ToList();


        //var students = db.Students.FirstOrDefault(c => c.User == user && c.Sports.Id == sport);
        //ViewBag.student = students;

        ///*var mappedOptions = students.Select(o => new { exp = o.Experience.ToString(), aexp = o.AboutExpireance.ToString() });*/

        //if (students == null)
        //{
        //    // Return the mapped options as a JSON response
        //    return Ok(("null"));
        //}
        //else
        //{
        //    return Json(new { exp = students.Experience.ToString(), aexp = students.AboutExpireance });
        //}
    }

    [HttpGet]
    [Route("/Cource/GetCourcesNamesAsync")]
    public async Task<IActionResult> GetCourcesNamesAsync()
    {

        var items = await _context.CourceNames.ToListAsync();
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


}