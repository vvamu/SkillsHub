﻿using SkillsHub.Domain.Models;
namespace SkillsHub.Application.Services.Interfaces;

public interface ILessonTypeService  : IAbstractLogModel<LessonType>
{
    public IQueryable<LessonType> GetAll();

    public Task<LessonType>? GetAsync(Guid itemId);
    public Task<LessonType> CreateAsync(LessonType item, Guid[] paymentCategories);
    public Task<LessonType> UpdateAsync(LessonType item, Guid[] paymentCategories);
    public Task<LessonType> RemoveAsync(Guid itemId);
    public Task<LessonType> RestoreAsync(Guid itemId);

}