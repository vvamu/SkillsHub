using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Application.Validators.LessonTypeModels;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Domain.Models.NotInUse;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

//For Admin Panel
public class PaymentCategoryService : AbstractLogModelService<PaymentCategory>
{
    private readonly ILessonTypeService _lessonTypeService;

    public PaymentCategoryService(ApplicationDbContext context, ILessonTypeService lessonTypeService) 
    {
        _context = context;
        _validator = new PaymentCategoryValidator();
        _contextModel = _context.PaymentCategories;
        _lessonTypeService = lessonTypeService;
    }

    public override async Task<PaymentCategory> UpdateAsync(PaymentCategory item)
    {
        var lessonTypes = _contextModel.Include(x => x.LessonTypePaymentCategory).ThenInclude(x=>x.LessonType).SelectMany(x => x.LessonTypePaymentCategory).Where(x=>x.LessonType!= null && x.LessonType.ParentId == null);
        var res = await base.UpdateAsync(item);
        var dbLessonTypes = lessonTypes.Select(x => new LessonTypePaymentCategory() { Id = x.Id, LessonTypeId = x.LessonTypeId, PaymentCategoryId = res.Id });
        _context.LessonTypePaymentCategories.UpdateRange(dbLessonTypes);
        await _context.SaveChangesAsync();
        return res;


    }
    public override async Task<bool> IsHardDelete(IQueryable<PaymentCategory> items)
    {
        var itemsIds = items.Select(x => x.Id).ToList();
        var oo = await _context.LessonTypePaymentCategories.Where(x => itemsIds.Contains((Guid)x.PaymentCategoryId)).ToListAsync();

        if (oo == null || oo.Count() == 0) return true; //throw new Exception("There is a reference to the lesson type. Please remove the reference before deleting the item");
        //else throw new Exception("Нельзя удалить абонемент, ведь он ссылается на одну из категорий занятий.");
        //var item = await items.LastOrDefaultAsync();

        //item.IsDeleted = !item.IsDeleted;
        //_contextModel.Update(item);
        return false;

    }


}
