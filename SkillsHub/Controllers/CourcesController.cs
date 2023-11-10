using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Persistence;

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
}
