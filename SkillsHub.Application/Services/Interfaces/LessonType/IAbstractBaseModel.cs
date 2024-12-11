namespace SkillsHub.Application.Services.Interfaces;
public interface IAbstractBaseModel<T>
{
	public Task<T> GetBaseAsync(Guid? id);
    public Task<IQueryable<T>> GetBaseAllAsync();
    public Task<T> BaseUpdateAsync(T item);
    public Task<T> BaseCreateAsync(T item);
	public Task<T> BaseRemoveAsync(Guid itemId);
    public Task<T> BaseRestoreAsync(Guid itemId);

}






