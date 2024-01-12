using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Helpers;
using SkillsHub.Persistence;

namespace SkillsHub.Controllers;

[Authorize]
public class FinancesController : Controller
{
	private readonly ApplicationDbContext _context;

	public FinancesController(ApplicationDbContext context)
	{
		_context = context;
	}
	[HttpGet]
	public async Task<IActionResult> Index()
	{
		var finances = await _context.Finances.Include(x=>x.Parent).ToListAsync();

		return View(finances);
	}
	[HttpGet]
	public async Task<IActionResult> GetFolders()
	{
		var finances = _context.Finances.Where(x => x.Budget == null).AsQueryable();
		return Json(JsonSerializerToAjax.GetJsonByIQueriable(finances));
	}
	[HttpPost]
	public async Task<IActionResult> Create(FinanceElement element)
	{
		await _context.AddAsync(element);
		await _context.SaveChangesAsync();
		return View("Index");
	}
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
		var elem = await _context.Finances.FindAsync(id);
            _context.Finances.Remove(elem);
            await _context.SaveChangesAsync();
            return View("Index");
        }
        public IActionResult GetCreateElementModal()
	{
		return PartialView("_CreateElement");
	}
	public IActionResult GetCreateFolderModal()
	{
		return PartialView("_CreateFolder");
	}
}
