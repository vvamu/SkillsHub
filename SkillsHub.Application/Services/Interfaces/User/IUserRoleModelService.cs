using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Interfaces;
public interface IUserRoleModelService<T> where T : class
{
    public Task<T> GetAsync(Guid itemId);
    
   public Task<IQueryable<T>> GetAllAsyncWithUserAndPossibleCoursesLink();

    public Task<IQueryable<T>> GetAllAsync();
    public Task<T> UpdateAsync(T item, Guid[] lessonTypeId);
    public Task<T> CreateAsync(T item, Guid[] lessonTypeId);
}
