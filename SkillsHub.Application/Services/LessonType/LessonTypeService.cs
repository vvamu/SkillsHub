﻿using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators.LessonTypeModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

//For Admin Panel
public class LessonTypeService : AbstractLogModelService<LessonType>, ILessonTypeService
{
    private readonly INotificationService _notificationService;

    protected override DbSet<LessonType> _contextModel => _context.LessonTypes;

    protected override FluentValidation.IValidator<LessonType> _validator => new LessonTypeValidator();

    protected override IQueryable<LessonType>? _fullInclude => _context.LessonTypes
            .Include(x => x.Course)
            .Include(x => x.LessonTypePaymentCategory).ThenInclude(x => x.PaymentCategory)
            .Include(x => x.GroupType)
            .Include(x => x.Location)
            .Include(x => x.AgeType)
            .Include(x => x.Groups);

    public LessonTypeService(ApplicationDbContext context, INotificationService notificationService) : base(context) { }
    public IQueryable<LessonType> GetAll()
    {

        foreach (var item in _fullInclude)
        {
            //item.Children = GetAllParents((Guid)item.Id).Where(x=>x.Id != item.Id).ToList();
            //item.Parents = GetAllChildren((Guid)item.Id).Where(x => x.Id != item.Id).ToList();

        }


        return _fullInclude;

    }


    /*
    public override async Task<LessonType>? GetAsync(Guid? itemId, bool withParents = false)
    {
        ///
        var item = await _context.LessonTypes
            .Include(x => x.Course)
            .Include(x => x.LessonTypePaymentCategory).ThenInclude(x => x.PaymentCategory)
            .Include(x => x.GroupType)
            .Include(x => x.Location)
            .Include(x => x.AgeType)
            .FirstOrDefaultAsync(x=>x.Id == itemId);

        //item.Children = GetAllParents((Guid)item.Id).Where(x => x.Id != item.Id).ToList();
        item.Parents = GetAllChildren((Guid)item.Id).Where(x => x.Id != item.Id).ToList();

        for (int i = 0; i < item.Parents.Count; i++)
        {
            item.Parents[i] = await _context.LessonTypes
                .Include(x => x.Course)
                .Include(x => x.LessonTypePaymentCategory).ThenInclude(x => x.PaymentCategory)
                .Include(x => x.GroupType)
                .Include(x => x.Location)
                .Include(x => x.AgeType)
                .Include(x => x.Groups)
                .FirstOrDefaultAsync(x => x.Id == item.Parents[i].Id);
        }
       
    ///
        var item = await base.GetAsync(itemId, withParents);
        return item ?? throw new Exception("Not found");
    }*/

    public async Task<LessonType> CreateAsync(LessonType item, Guid[] paymentCategories)
    {
        var payments = await CheckCorrectPaymentCategories(paymentCategories, item.IsActive);
        var res = await base.CreateAsync(item);
        var lessonTypePayments = payments.Select(x => new LessonTypePaymentCategory() { PaymentCategoryId = x.Id, LessonTypeId = item.Id });
        await _context.LessonTypePaymentCategories.AddRangeAsync(lessonTypePayments);
        await _context.SaveChangesAsync();
        /*
        var durationName =
            {
                get => DurationTypeValue;
        set
        {
            DurationTypeValue = value;
            if (!string.IsNullOrEmpty(DurationTypeName) && DurationTypeName == )
            {
                switch (DurationTypeName)
                {
                    case "lesson": { CountWorkingHours = DurationTypeValue * LessonTimeInMinutes; break; }
                    case "month": { CountWorkingHours = DurationTypeValue * LessonTimeInMinutes; break; }


                }
                Count
                    }
        }
    }*/

        return res;

    }


    public async Task<LessonType> RestoreAsync(Guid itemId)
    {
        return await base.RestoreAsync(itemId);
    }

    public async Task<LessonType> UpdateAsync(LessonType item, Guid[] paymentCategories, bool withStudents = true, bool withTeachers = true, object passedObject = null)
    {
        LessonType olItemDb = null;
        //if (paymentCategories != null)
            olItemDb = await GetLastValueAsync(item.Id, touchFullInclude: false) ?? throw new Exception("Lesson type not found");
        //else
        //    olItemDb = await GetAsync(item.Id) ?? throw new Exception("Lesson type not found");

        var payments = await CheckCorrectPaymentCategories(paymentCategories, item.IsActive);

        if (AreObjectsDifferent(olItemDb, item))
        {
            await CheckCorrectActiveProperties(item.AgeTypeId, item.CourseId, item.GroupTypeId, item.LocationId, passedObject);
            item.LessonTypePaymentCategory = null;
            item = await base.UpdateAsync(item);
        }

        var lessonTypePayments = payments.Select(x => new LessonTypePaymentCategory() { PaymentCategoryId = x.Id, LessonTypeId = item.Id });
        await _context.LessonTypePaymentCategories.AddRangeAsync(lessonTypePayments);
        await _context.SaveChangesAsync();






        //if (withStudents)
        //{
        //    var oldLessonTypeStudents = await _context.LessonTypeStudents.Where(x => x.LessonTypeId == olItemDb.Id).ToListAsync();
        //    oldLessonTypeStudents.ForEach(x => x.LessonTypeId = item.Id);
        //    _context.LessonTypeStudents.UpdateRange(oldLessonTypeStudents);
        //    await _context.SaveChangesAsync();

        //    //var lessonTypeStudents = oldLessonTypeStudents.Select(x => new LessonTypeStudent() { StudentId = x.StudentId, LessonTypeId = item.Id });
        //    //await _context.LessonTypeStudents.AddRangeAsync(lessonTypeStudents);
        //}
        //if (withTeachers)
        //{
        //    var oldLessonTypeTeachers = await _context.LessonTypeTeachers.Where(x => x.LessonTypeId == olItemDb.Id).ToListAsync();
        //    oldLessonTypeTeachers.ForEach(x => x.LessonTypeId = item.Id);
        //    _context.LessonTypeTeachers.UpdateRange(oldLessonTypeTeachers);
        //    await _context.SaveChangesAsync();

        //    //var lessonTypeTeachers = oldLessonTypeTeachers.Select(x => new LessonTypeTeacher() { TeacherId = x.TeacherId, LessonTypeId = item.Id });
        //    //await _context.LessonTypeTeachers.AddRangeAsync(lessonTypeTeachers);
        //    //await _context.SaveChangesAsync();
        //}



        /*
        var oldPayemntCatogories = olItemDb?.LessonTypePaymentCategory?.Where(x=>x.PaymentCategoryId != null).Select(x=>x.PaymentCategoryId ?? Guid.Empty).ToList() ?? new List<Guid>(); 
        var toUpdate = paymentCategories.Intersect(oldPayemntCatogories).ToList();
        var toCreate = paymentCategories.Except(oldPayemntCatogories).ToList();
        var toDelete = oldPayemntCatogories.Except(paymentCategories).ToList();
        
        foreach (var it in toDelete)
        {
            //var del = await _context.LessonTypePaymentCategories.FirstOrDefaultAsync(x=>x.PaymentCategoryId == it &&  x.LessonTypeId == item.Id);
            //_context.LessonTypePaymentCategories.Remove(del);
        }
        foreach (var it in toUpdate)
        {
            
            if (!AreObjectsDifferent(olItemDb, item)) break;
            var update = await _context.LessonTypePaymentCategories.FirstOrDefaultAsync(x => x.PaymentCategoryId == it && x.LessonTypeId == item.Id);
            update.LessonTypeId = item.Id;
            _context.LessonTypePaymentCategories.Update(update);
            
        }
        */
        //await _context.SaveChangesAsync();

        return item;

    }


    public async Task<LessonType> RemoveAsync(Guid itemId)
    {
        var item = await _contextModel.FindAsync(itemId);

        var res = await base.RemoveAsync(itemId);

        var parents = GetAllChildren(itemId);
        var isHard = await IsHardDelete(parents.AsQueryable());
        if (isHard)
        {
            var lessonTypesPayments = _context.LessonTypePaymentCategories.Where(x => x.LessonTypeId == itemId);
            _context.LessonTypePaymentCategories.RemoveRange(lessonTypesPayments);
            await _context.SaveChangesAsync();
        }
        else
        {
            item.IsActive = false;
            _context.LessonTypes.Update(item);
            await _context.SaveChangesAsync();
        }

         return res;
    }

    public override async Task<LessonType> GetLastValueAsync(Guid? itemId, bool withParents = false, bool touchFullInclude = true)
    {
        if (itemId == Guid.Empty) return new LessonType();
        var res = await GetAsync(itemId, withParents);
        if (res.ParentId != null)
        {
            res = GetAllParents(res.Id).OrderBy(x => x.DateCreated).LastOrDefault();
            res = await GetAsync(res.Id, withParents);
        }
        return res;
    }

    public override async Task<bool> IsHardDelete(IQueryable<LessonType> items)
    {
        //var oo = _contextModel.AsQueryable();

        //var itemsWithRefs = await items.Include(x=>x.Groups).Where(x=>x.Groups != null && x.Groups.Count() > 0).ToListAsync();
        var children = items.AsEnumerable().ToList();

        for (int i = 0; i < children.Count; i++)
        {
            children[i] = await _context.LessonTypes
                /*.Include(x => x.Course)
                .Include(x => x.LessonTypePaymentCategory).ThenInclude(x => x.PaymentCategory)
                .Include(x => x.GroupType)
                .Include(x => x.Location)
                .Include(x => x.AgeType*/
                .Include(x => x.Groups)
                .Include(x => x.LessonTypeStudents)
                .Include(x => x.LessonTypeTeachers)
                .FirstOrDefaultAsync(x => x.Id == children[i].Id);
            if (children[i] != null && children[i].Groups != null && children[i].Groups.Count() > 0) return false;
            if (children[i] != null && children[i].LessonTypeStudents != null && children[i].LessonTypeStudents.Count() > 0) return false;
            if (children[i] != null && children[i].LessonTypeTeachers != null && children[i].LessonTypeTeachers.Count() > 0) return false;
        }



        //var itemssss = _context.LessonTyp


        /*var itemsWithRefs = lessonTypes.Where(x => x.AgeTypeId != null
        || x.CourseId != null
        || x.GroupTypeId != null
        || x.LocationId != null
        || x.PaymentCategories != null).ToList();*/

        //if (itemsWithRefs == null || itemsWithRefs.Count() == 0) return true; //throw new Exception("There is a reference to the lesson type. Please remove the reference before deleting the item");




        return true;

    }

    public async Task<List<PaymentCategory>> CheckCorrectPaymentCategories(Guid[] paymentCategories, bool itemIsActive)
    {
        List<PaymentCategory> result = new List<PaymentCategory>();
        if (paymentCategories.Length == 0 && itemIsActive) throw new Exception("Before create active lesson type choose payment category before it");
        foreach (var it in paymentCategories)
        {
            var r = await _context.PaymentCategories.FirstOrDefaultAsync(x => x.Id == it);
            if (r == null) throw new Exception("Payment category not found");
            if (r.IsDeleted) throw new Exception($"{it} is deleted");
            result.Add(r);

        }
        return result;
    }

    public async Task CheckCorrectActiveProperties(Guid? ageTypeId, Guid? courseId, Guid? groupTypeId, Guid? locationId, object passedObject = null)
    {
        bool isFound = false;
        if (!(passedObject is AgeType))
        {
            isFound = _context.AgeTypes.FirstOrDefault(x => x.Id == ageTypeId && !x.IsDeleted) != null ? true : throw new Exception("Age type now was deleted");
        }
        if (!(passedObject is Course))
        {
            isFound = _context.Courses.FirstOrDefault(x => x.Id == courseId && !x.IsDeleted) != null ? true : throw new Exception("Course now was deleted");
        }
        if (!(passedObject is GroupType))
        {
            isFound = _context.GroupTypes.FirstOrDefault(x => x.Id == groupTypeId && !x.IsDeleted) != null ? true : throw new Exception("Group type now was deleted");
        }
        if (!(passedObject is Location))
        {
            isFound = _context.Locations.FirstOrDefault(x => x.Id == locationId && !x.IsDeleted) != null ? true : throw new Exception("Location type now was deleted");
        }

    }


}
