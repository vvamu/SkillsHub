using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Helpers.SearchModels;
using SkillsHub.Persistence;

namespace SkillsHub.Controllers;

[Authorize]
public class PaymentCategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IAbstractLogModelService<PaymentCategory> _paymentCategoryService;

    public PaymentCategoryController(ApplicationDbContext context, IAbstractLogModelService<PaymentCategory> paymentCategoryService)
    {
        _context = context;
        _paymentCategoryService = paymentCategoryService as PaymentCategoryService;

    }

    [HttpPost]
    public async Task<IActionResult> OptionsList(string paymentCategoryId)
    {
        var paymentCategories = _context.PaymentCategories.AsAsyncQueryable();
        var idVal = Guid.TryParse(paymentCategoryId?.ToString(), out var result) ? result : Guid.Empty;

        return PartialView((paymentCategories, idVal));
    }



    public async Task<IActionResult> Index()
    {
        var items = _paymentCategoryService.GetItems(onlyCurrent:true);
        var result = await items.Where(x => x.ParentId == null).ToListAsync();
        return PartialView(result);
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid? itemId)
    {
        var res = await _paymentCategoryService.GetLastValueAsync(itemId);
        return View(res);
    }
    [HttpPost]
    public async Task<IActionResult> Create(PaymentCategory item)
    {
        PaymentCategory? result = null;
        try
        {
            if (item.DurationTypeStudentValue == 0) item.DurationTypeStudentValue = 1;
            if (item.DurationTypeTeacherValue == 0) item.DurationTypeTeacherValue = 1;
            var isEdit = await _context.PaymentCategories.FindAsync(item.Id);
            if (isEdit != null) result = await _paymentCategoryService.UpdateAsync(item);
            else result = await _paymentCategoryService.CreateAsync(item);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(result); }
        if (result == null) return RedirectToAction("InstitutionSetting", "CRM");
        return View(result);
    }

    public async Task<IActionResult> Remove(PaymentCategory item)
    {
        try
        {
            var res = await _paymentCategoryService.RemoveAsync(item.Id);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(item); }
        return RedirectToAction("InstitutionSetting", "CRM");
    }

    [HttpGet]
    public async Task<IActionResult> OptionsList(Guid lessonTypeId)
    {
        if (lessonTypeId == Guid.Empty) return BadRequest("First you need to select lesson type");
        var paymentsByLessonType = _context.LessonTypePaymentCategories.Where(x => x.LessonTypeId == lessonTypeId).Select(x => x.PaymentCategoryId);
        var payments = await _context.PaymentCategories.Where(x => (x.ParentId == null || x.ParentId == Guid.Empty) && !x.IsDeleted).Where(x => paymentsByLessonType.Contains(x.Id)).ToListAsync();
        if (payments == null || payments.Count == 0) return Ok(new List<PaymentCategory>());
        return Ok(payments);
    }

    /*
    [HttpGet]
	public async Task<IActionResult> OptionsList(Guid itemId)
	{
		var items = _context.PaymentCategories;
		return PartialView("OptionsList", (items, itemId));
	}
    */
}
