namespace SkillsHub.Application.Services.Interfaces;
public interface IAbstractLogModelService<T>
{
    public Task<T> GetLastValueAsync(Guid? itemId, bool withParents = false,bool touchFullInclude =true);
    public IQueryable<T> GetItems(bool onlyCurrent = false,bool withParents = false, bool touchFullInclude = false);
    public Task<T> UpdateAsync(T item);
    public Task<T> CreateAsync(T item);
    public Task<T> RemoveAsync(Guid itemId);
    public Task<T> RestoreAsync(Guid itemId);
    

}







