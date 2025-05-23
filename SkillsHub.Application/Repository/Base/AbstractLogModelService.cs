﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Helpers;

public abstract class AbstractLogModelService<T> : AbstractTransactionService, IAbstractLogModelService<T> where T : LogModel<T>, new()
{
    protected abstract DbSet<T> _contextModel { get; }
    protected abstract IValidator<T> _validator { get; }
    protected abstract IQueryable<T>? _fullInclude { get; }

    //protected abstract DbSet<T> _contextModel { get; }
    //protected abstract IValidator<T> _validator { get;}
    //protected abstract IQueryable<T>? _fullInclude { get; }

    protected AbstractLogModelService(ApplicationDbContext context): base(context)
    {
    }

    public virtual async Task<T> GetLastValueAsync(Guid? itemId, bool withParents = false, bool touchFullInclude = true)
    {
        if (itemId == Guid.Empty) return new T();
        var res = await GetAsync(itemId, withParents);
        if (res.ParentId != null)
        {
            res = GetAllParents(res.Id).OrderBy(x => x.DateCreated).LastOrDefault();
            res = await GetAsync(res.Id, withParents);
        }
        return res;
    }
    public IQueryable<T> GetItems(bool onlyCurrent = false, bool withParents = false , bool touchFullInclude = false)
    {
        IQueryable<T> items;
        if (touchFullInclude) items = _fullInclude; else items = _contextModel.AsQueryable();
        if (onlyCurrent) items = items.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
        else items = items;
        if(withParents)
        {
            foreach (var item in items)
            {
                //item.Children = GetAllParents((Guid)item.Id).Where(x => x.Id != item.Id).ToList();
                item.Parents = GetAllChildren((Guid)item.Id).Where(x => x.Id != item.Id).ToList();
            }
        }

        return items;
    }
    public async Task<T> CreateAsync(T item)
    {
        var itemId = Guid.Empty; if (item.Id != null) itemId = item.Id;

        var oldValue = _contextModel.Find(itemId) ?? new T();
        await Validate(oldValue, item);

        item.Id = Guid.Empty;
        item.DateCreated = DateTime.Now;
        var res = await _contextModel.AddAsync(item);


        var children = GetAllParents(oldValue.Id).OrderByDescending(x => x.DateCreated).ToList();
        var parents = GetAllChildren(oldValue.Id).OrderByDescending(x => x.DateCreated).ToList();

        if (oldValue.Id != Guid.Empty) parents.Insert(0, oldValue);
        if (children.Count() != 0)
        {
            res.Entity.DateRegistration = parents.LastOrDefault()?.DateCreated;
        }
        else
        {
            res.Entity.DateRegistration = res.Entity.DateCreated;
        }


        await _context.SaveChangesAsync();
        return res.Entity;
    }
    public virtual async Task<T> UpdateAsync(T item)
    {
        var itemDb = _contextModel.AsNoTracking().FirstOrDefault(x => x.Id == item.Id) ?? throw new Exception("No info in database");
        var resCreated = await CreateAsync(item);

        itemDb.ParentId = resCreated.Id;
        _contextModel.Update(itemDb);
        await _context.SaveChangesAsync();      
        return await GetAsync(resCreated.Id);
    }
    public async Task<T> RemoveAsync(Guid itemId)
    {
        var itemDb = _contextModel.FirstOrDefault(x => x.Id == itemId) ?? throw new Exception("No info in database");
        var children = GetAllChildren(itemId).ToList();
        var isHardDelete = await IsHardDelete(children.AsQueryable());
        if (isHardDelete)
        {
            _contextModel.RemoveRange(children);
            if (_contextModel is DbSet<LessonType>)
            {
                var lessonTypesPayments = _context.LessonTypePaymentCategories.Where(x => x.LessonTypeId == itemId);
                _context.LessonTypePaymentCategories.RemoveRange(lessonTypesPayments);
                await _context.SaveChangesAsync();
                return await GetAsync(itemDb.Id);
            }
        }
        else
        {
            itemDb.IsDeleted = true;
            await UpdateAsync(itemDb);
        }

           
        await _context.SaveChangesAsync();
        return await GetAsync(itemDb.Id);
    }
    public async Task<T> RestoreAsync(Guid itemId)
    {
        var itemDb = _contextModel.FirstOrDefault(x => x.Id == itemId) ?? throw new Exception("No info in database");
        itemDb.IsDeleted = false;
        _contextModel.Update(itemDb);
        await _context.SaveChangesAsync();
        return await GetAsync(itemDb.Id);
    }
    public virtual async Task<bool> IsHardDelete(IQueryable<T> items)
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

        //Check unique rows without considering parent rows
        var resultSearching = await _contextModel.Where(x => x.ParentId == null || x.ParentId == Guid.Empty).ToListAsync();
        //resultSearching = resultSearching.Where(x => !children.Select(x => x.Id).Contains(x.Id)).ToList();
        resultSearching = resultSearching.Where(x => x.Equals(newItem)).ToList();
        if (resultSearching.Count() > 0 && resultSearching.FirstOrDefault().Id != oldValue.Id) throw new Exception("Entity with those properties already defined");

        if (oldValue == null) return;
        var children = GetAllChildren(oldValue.Id).OrderByDescending(x => x.DateCreated).ToList();
        if (children == null || children.Count == 0) return;
        var equals = AreObjectsDifferent(oldValue, newItem);
        if (!equals) throw new Exception("No changes");
        if (newItem is AgeType || newItem is GroupType) { }
        //else if (oldValue.Equals(newItem)) throw new Exception("No changes");

    }

    public virtual async Task UploadImage()
    {
        throw new NotImplementedException();
    }

    #region Helpers

    protected virtual async Task<T> GetAsync(Guid? itemId, bool withParents = false)
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
        item.Children = GetAllParents((Guid)itemId).Where(x => x.Id != itemId).ToList();
        item.Parents = GetAllChildren((Guid)itemId).Where(x => x.Id != itemId).ToList();

        if (withParents && _fullInclude != null)
        {
            if (item.Parents != null)
            {
                List<T> parentResult = new List<T>();

                foreach (var par in item.Parents)
                {
                    parentResult.Add(await _fullInclude.FirstOrDefaultAsync(x => x.Id == (Guid)par.Id));
                }
                item.Parents = parentResult;
            }
        }


        return item;
    }
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
    public IEnumerable<T> GetAllParents(Guid childId)
    {
        var child = _contextModel.FirstOrDefault(i => i.Id == childId);

        if (child != null)
        {
            yield return child;
            if (child.ParentId != null)
            {
                foreach (var parent in GetParentsRecursive((Guid)child.ParentId))
                {
                    yield return parent;
                }
            }
        }
    }

    private IEnumerable<T> GetParentsRecursive(Guid? childParentId)
    {
        var parent = _contextModel.FirstOrDefault(i => i.Id == childParentId);

        if (parent != null)
        {
            yield return parent;
            if (parent.ParentId != null)
            {
                foreach (var grandParent in GetParentsRecursive((Guid)parent.ParentId))
                {
                    yield return grandParent;
                }
            }
        }
    }


    public IEnumerable<T> GetAllChildren(Guid parentId)
    {
        var parent = _contextModel.FirstOrDefault(i => i.Id == parentId);

        if (parent != null)
        {
            yield return parent;

            foreach (var child in GetChildrenRecursive(parent.Id))
            {
                yield return child;
            }
        }
    }
    private IEnumerable<T> GetChildrenRecursive(Guid parentId)
    {
        var children = _contextModel.Where(i => i.ParentId == parentId).ToList();

        foreach (var child in children)
        {
            yield return child;
            foreach (var grandChild in GetChildrenRecursive(child.Id))
            {
                yield return grandChild;
            }
        }
    }

    #endregion

}

