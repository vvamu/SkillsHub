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
public class SubjectController : Controller
{
	private readonly ApplicationDbContext _context;


	public SubjectController(ApplicationDbContext context,IPaymentCategoryService paymentCategoryService)
	{
		_context = context;
	}
	[HttpGet]
	public async Task<IActionResult> Index()
	{
		var finances = await _context.Finances.Include(x=>x.Parent).ToListAsync();

		return PartialView(finances);
	}

	[HttpPost]
    public async Task<IActionResult> OptionsList(string? subjectId )
    {
		
        var ages = _context.Subjects.AsAsyncQueryable();
        var idVal = Guid.TryParse(subjectId?.ToString(), out var result) ? result : Guid.Empty;
		return PartialView((ages, idVal));
    }
}
