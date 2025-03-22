using SkillsHub.Application.DTO;
using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Application.Services.Repository;

public interface IUserRepository
{
    public  Task<ApplicationUser> GetAsync(Guid id);
    public  Task<IQueryable<ApplicationUser>> GetItemsAsync();
    public  Task<ApplicationUser> CreateAsync(UserCreateDTO item);
    public  Task<ApplicationUser> UpdateAsync(UserCreateDTO item);
    public Task<ApplicationUser> SoftDeleteAsync(ApplicationUser item);
}