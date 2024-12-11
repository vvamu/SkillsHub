using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Helpers;

public abstract class AbstractBaseModelService<T> : AbstractTransactionService, IAbstractBaseModel<T> where T : SkillsHub.Domain.BaseModels.BaseEntity, new()
{
    protected DbSet<T> _contextModel;
    protected IValidator<T> _validator { get; set; }
    protected IQueryable<T>? _fullInclude;

    public async Task<T> GetBaseAsync(Guid? itemId)
    {
        if (itemId == Guid.Empty) return new T();
        T? item = null;
        if (_fullInclude != null)
        {
            item = await _fullInclude.FirstOrDefaultAsync(x => x.Id == itemId);
        }
        else
            item = await _contextModel.FirstOrDefaultAsync(x => x.Id == itemId);
        if (item == null) return new T();
       
        return item;
    }

    public async Task<IQueryable<T>> GetBaseAllAsync()
    {
        if (_fullInclude != null)
        {
            return _fullInclude.AsQueryable();
        }
        else
           return _contextModel.AsQueryable();
    }


    public async Task<T> BaseUpdateAsync(T item)
    {
        var itemDb = _contextModel.FirstOrDefault(x => x.Id == item.Id) ?? throw new Exception("No info in database");
        await Validate(null, item);
        _contextModel.Update(itemDb);
        await _context.SaveChangesAsync();
        return await GetBaseAsync(item.Id);
    }

    public async Task<T> BaseCreateAsync(T item)
    {
        var itemId = Guid.Empty;
        if (item.Id != null) itemId = item.Id;
        var oldValue = _contextModel.Find(itemId) ?? new T();
        await Validate(oldValue, item);
        item.Id = Guid.Empty;
        item.DateCreated = DateTime.Now;
        var res = await _contextModel.AddAsync(item);
        await _context.SaveChangesAsync();
        return res.Entity;
    }

    public async Task<T> BaseRemoveAsync(Guid itemId)
    {
        var itemDb = _contextModel.FirstOrDefault(x => x.Id == itemId) ?? throw new Exception("No info in database");
       

        //var parents = GetAllChildren(itemId).ToList();
        var isHardDelete = await IsHardDelete();
        if (isHardDelete)
        {
            _contextModel.Remove(itemDb);

            //var payments = _context.LessonTypePaymentCategories.Where(x=>x.LessonTypeId == itemId).ToList();
            //_context.LessonTypePaymentCategories.RemoveRange(payments);

        }
        else
        {
            itemDb.IsDeleted = true;
            await BaseUpdateAsync(itemDb);
            //_contextModel.Update(itemDb);
        }

        await _context.SaveChangesAsync();
        return await GetBaseAsync(itemDb.Id);
    }

    public async Task<T> BaseRestoreAsync(Guid itemId)
    {
        var itemDb = _contextModel.FirstOrDefault(x => x.Id == itemId) ?? throw new Exception("No info in database");
        itemDb.IsDeleted = false;
        _contextModel.Update(itemDb);
        await _context.SaveChangesAsync();
        return await GetBaseAsync(itemDb.Id);
    }


    public virtual async Task<bool> IsHardDelete(IQueryable<T> items = null)
    {
        return true;
    }


    public virtual async Task Validate(T oldValue, T newItem)
    {
        if (_validator != null)
        {
            var userValidationResult = await _validator.ValidateAsync(newItem);
            if (!userValidationResult.IsValid)
            {
                var errors = userValidationResult.Errors;
                var errorsString = string.Concat(errors);
                throw new Exception(errorsString);
            }
        }
        if (oldValue == null) return;
        //Check unique rows without considering parent rows
        var resultSearching = await _contextModel.ToListAsync();
        resultSearching = resultSearching.Where(x => !resultSearching.Select(x => x.Id).Contains(x.Id)).ToList();
        resultSearching = resultSearching.Where(x => x.Equals(newItem)).ToList();
        //if (resultSearching.Count() > 1) throw new Exception("Entity with those properties already defined");
        if (!AreObjectsDifferent(oldValue, newItem)) throw new Exception("No changes");

    }

    #region Helpers

    protected bool AreObjectsDifferent(T oldValue, T newItem)
    {
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var oldValuePropValue = property.GetValue(oldValue);
            var newItemPropValue = property.GetValue(newItem);

            if (oldValuePropValue == null && newItemPropValue == null)
            {
                continue; // Both values are null, move to the next property
            }

            if ((oldValuePropValue == null && newItemPropValue != null) || (oldValuePropValue != null && newItemPropValue == null) || !oldValuePropValue.Equals(newItemPropValue))
            {
                return true;
            }
        }

        return false;
    }
    #endregion
    /*

    public virtual async Task Validate(T oldValue, T newItem)
    {
        if(_validator != null)
        {
            var userValidationResult = await _validator.ValidateAsync(newItem);
            if (!userValidationResult.IsValid)
            {
                var errors = userValidationResult.Errors;
                var errorsString = string.Concat(errors);
                throw new Exception(errorsString);
            }
        }
        if (oldValue == null) return;
        var children = GetAllChildren(oldValue.Id).OrderByDescending(x => x.DateCreated).ToList();
        if (children == null || children.Count == 0) return;
        //Check unique rows without considering parent rows
        var resultSearching = await _contextModel.Where(x => x.Parent == null).ToListAsync();
        resultSearching = resultSearching.Where(x => !children.Select(x => x.Id).Contains(x.Id)).ToList();
        resultSearching = resultSearching.Where(x => x.Equals(newItem)).ToList();
        //if (resultSearching.Count() > 1) throw new Exception("Entity with those properties already defined");
        if (!AreObjectsDifferent(oldValue, newItem)) throw new Exception("No changes");

    }


    #endregion
    */
}

