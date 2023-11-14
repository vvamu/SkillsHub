using SkillsHub.Application.DTO;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Interfaces;

public interface IUserService
{
    public Task InitialCreateAsync();
    public Task<Teacher> CreateTeacherAsync(Guid userId, TeacherDTO item);
    public Task<ApplicationUser> CreateUserAsync(UserCreateDTO user);
    public Task<ApplicationUser?> GetCurrentUserAsync();
    public Task<ApplicationUser> SignInAsync(UserLoginDTO item);
    public Task<ApplicationUser> SignInAsync(ApplicationUser item);
    public Task<ApplicationUser> GetUserByIdAsync(Guid id);
    public Task<bool> IsAdminAsync();

    public Task<IQueryable<ApplicationUser>> GetAllAsync();

    public Task<IQueryable<StudentDTO>> GetAllStudentsAsync();

}
