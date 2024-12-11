namespace SkillsHub.Application.Services.Interfaces;
public interface IAbstractLogModel<T>
{
	public Task<T> GetAsync(Guid? id, bool withParents = false , bool touchFullInclude = true);
    public Task<T> GetLastValueAsync(Guid? itemId, bool withParents = false);

    public Task<IQueryable<T>> GetAllAsync();
    public IQueryable<T> GetAllItemsToList();
    public Task<T> UpdateAsync(T item);
    public Task<T> CreateAsync(T item);
	public Task<T> RemoveAsync(Guid itemId);

    public Task<T> RestoreAsync(Guid itemId);

}






