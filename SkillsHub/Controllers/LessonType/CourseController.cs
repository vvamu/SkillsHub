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
using SkillsHub.Helpers;
using SkillsHub.Models;
using SkillsHub.Persistence;
using System.Diagnostics;
using System.Text;
using static Viber.Bot.NetCore.Models.ViberResponse;

namespace SkillsHub.Controllers;
public class CourseController : Controller
{
    private readonly IAbstractLogModel<Course> _courseService;
    private readonly ApplicationDbContext _context;

	public CourseController(ApplicationDbContext context, IAbstractLogModel<Course> courseService)
    {
        _courseService = courseService as CourseService;
        _context = context;

	}

    public async Task<IActionResult> Index()
    {
        var items = _courseService.GetAllItemsToList();
        var result = await items.ToListAsync();
        return PartialView(result);
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid? itemId)
    {
        var res = await _courseService.GetLastValueAsync(itemId);
        return View(res);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Course item)
    {
        Course? result;
        try
        {
            var id = Guid.Empty;
            Guid.TryParse(item.StringId, out id); 
            if(item.Id == Guid.Empty && id != Guid.Empty)  item.Id = id;
            var isEdit = await _context.Courses.FindAsync(item.Id);
            if (isEdit != null) result = await _courseService.UpdateAsync(item);
            else result = await _courseService.CreateAsync(item);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(item); }
        var res = await _courseService.GetLastValueAsync(result.Id);
        return View(res);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeActiveStatus(Course item,bool isRemove = false)
    {
        try
        {
            var id = Guid.Empty;
            Guid.TryParse(item.StringId, out id);
            if (item.Id == Guid.Empty && id != Guid.Empty) item.Id = id;
            if(isRemove) item = await _courseService.RemoveAsync(item.Id);
            else item = await _courseService.RestoreAsync(item.Id);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View("Create",item); }
        return RedirectToAction("Create", item);
    }
    /*
    [HttpPost]
    public async Task<IActionResult> OptionsList(string subjectId,string courseId)
    {
        var courses = _context.Courses.AsAsyncQueryable();
        var subjId = Guid.TryParse(subjectId?.ToString(), out var result) ? result : Guid.Empty;
        var coursId = Guid.TryParse(courseId?.ToString(), out var resultC) ? resultC : Guid.Empty;

       // if (subjId != Guid.Empty) courses = courses.Where(x => x.SubjectId == subjId);

        return PartialView((courses, coursId));    
    }*/

}