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
public class PaymentCategoryController : Controller
{
	private readonly ApplicationDbContext _context;


	public PaymentCategoryController(ApplicationDbContext context,IPaymentCategoryService paymentCategoryService)
	{
		_context = context;
	}

    [HttpPost]
    public async Task<IActionResult> OptionsList(string paymentCategoryId)
    {
		var paymentCategories = _context.PaymentCategories.AsAsyncQueryable();
        var idVal = Guid.TryParse(paymentCategoryId?.ToString(), out var result) ? result : Guid.Empty;

        return PartialView((paymentCategories, idVal));
    }



    [HttpGet]
	public async Task<IActionResult> Index()
	{
		var finances = await _context.PaymentCategories.Include(x=>x.Parent).ToListAsync();

		return View(finances);
	}
	[HttpGet]
	public async Task<IActionResult> OptionsList(Guid itemId)
	{
		var items = _context.PaymentCategories;
		return PartialView("OptionsList", (items, itemId));
	}

    [HttpPost]
    public async Task<IActionResult> Edit(PaymentCategory model) //TODO
	{

		return PartialView("_Result");
	}
}
