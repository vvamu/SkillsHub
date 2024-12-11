using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
namespace SkillsHub.Application.Helpers;

public abstract class AbstractLessonTypeLogModelService<T> : AbstractLogModelService<T> where T : LessonTypePropertyModel<T>, new()
{
    protected ILessonTypeService _lessonTypeService;
    protected abstract void SetPropertyId(LessonType item, Guid value);
    public override async Task<T> UpdateAsync(T item)
    {
        var lessonTypes = await _contextModel.Where(x => x.Id == item.Id)
            .Include(x => x.LessonTypes).ThenInclude(x=>x.LessonTypePaymentCategory).Where(x => x.LessonTypes != null)
            .SelectMany(x => x.LessonTypes).ToListAsync();
        lessonTypes = lessonTypes.Where(x => x.ParentId == null || x.ParentId == Guid.Empty).ToList();

        var executionStrategy = CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await BeginTransactionAsync();
            try
            {

                var res = await base.UpdateAsync(item);
                await _context.SaveChangesAsync();

                foreach (var lessonType in lessonTypes)
                {
                    SetPropertyId(lessonType, res.Id);

                    var paymentCategoryIds = lessonType.LessonTypePaymentCategory != null ?
                        lessonType.LessonTypePaymentCategory.Select(x => x.PaymentCategoryId)
                                                .OfType<Guid>()
                                                .ToArray() :
                        Array.Empty<Guid>();

                    await _lessonTypeService.UpdateAsync(lessonType, paymentCategoryIds,passedObject:new T());
                }

                //_context.LessonTypes.UpdateRange(lessonTypes);
                //await _context.SaveChangesAsync();
                await CommitAsync(transaction);
                return res;

            }
            catch (Exception ex)
            {
                await RollbackAsync(transaction);
                throw new Exception("Не получилось обновить элемент. Попробуйте позже.", ex);
            }
        });
        return item;

        }

    public override async Task<bool> IsHardDelete(IQueryable<T> items)
    {
        foreach (var a in items)
        {
            var a2 = await _contextModel.Include(x => x.LessonTypes).Where(x => x.LessonTypes != null).FirstOrDefaultAsync(x => a.Id == x.Id);
            if (a2.LessonTypes != null &&  a2.LessonTypes.Count() > 0) return false;

        }

        //var oo = items.Include(x => x.LessonTypes).Where(x=>x.LessonTypes!= null).ToList();
        //if (oo == null || oo.Count() == 0) return true; //throw new Exception("There is a reference to the lesson type. Please remove the reference before deleting the item");
        
        
        //await Conte
        return true;

    }

}
