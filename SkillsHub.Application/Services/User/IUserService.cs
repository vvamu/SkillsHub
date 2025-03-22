using SkillsHub.Application.DTO;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Implementation.User;

public interface IUserService
{
    public Task<ApplicationUser?> GetCurrentUserAsync();
    public Task<UserCreateDTO> GetUserCreateDTOByIdAsync(Guid id);
    public Task<ApplicationUser> GetById(Guid id);
    public Task<IQueryable<ApplicationUser>> GetItems();
    public Task<IQueryable<NotificationMessage>> GetCurrentUserNotifications();

    public Task<ApplicationUser> SignInAsync(UserLoginDTO item);
    public Task<ApplicationUser> SignInAsync(ApplicationUser item);
    public Task SignOutAsync();

    public Task<ApplicationUser> Restore(ApplicationUser item);

    public Task<ApplicationUser> CreateAsync(UserCreateDTO item, List<string>? roles, bool baseUserInfoIsBase = false);
    public Task<ApplicationUser> UpdateAsync(UserCreateDTO item, List<string>? roles);
    public Task<ApplicationUser> DeleteAsync(ApplicationUser item, bool isHardDelete = false);
    public Task<ApplicationUser> DeleteAsync(Guid id, bool isHardDelete = false);



}
