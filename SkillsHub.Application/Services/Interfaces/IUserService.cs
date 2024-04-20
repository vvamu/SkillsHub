using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.DTO;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Interfaces;

public interface IUserService
{
    public Task InitialCreateAsync();
    public Task<ApplicationUser?> GetCurrentUserAsync();
    public Task<UserCreateDTO> GetUserCreateDTOByIdAsync(Guid id);
    public Task<ApplicationUser> GetUserByIdAsync(Guid id);
    public Task<IQueryable<ApplicationUser>> GetAllAsync();

    public Task<IQueryable<Student>> GetAllStudentsAsync();
    public IQueryable<Teacher> GetAllTeachers();


    public Task<ApplicationUser> SignInAsync(UserLoginDTO item);
    public Task<ApplicationUser> SignInAsync(ApplicationUser item);
    public Task SignOutAsync();


    public Task<Teacher> CreateTeacherAsync(ApplicationUser user, Teacher item);
    public Task<Student> CreateStudentAsync(ApplicationUser user, Student item);
    public Task<ApplicationUser> CreateUserAsync(UserCreateDTO user);
    public Task<ApplicationUser> UpdateUserAsync(UserCreateDTO item);
    /*
    public Task<Teacher> UpdateTeacherWithCourcesNames(Teacher item, List<Guid> courcesId);
    public Task<Student> UpdateStudentWithCourcesNames(Student item, List<Guid> courcesId);
    
    */
    
    public Task<ApplicationUser> HardDeleteAsync(ApplicationUser item);
    public Task<ApplicationUser> SoftDeleteAsync(ApplicationUser item);
    public Task<ApplicationUser> Restore(ApplicationUser item);

    public Task<IQueryable<NotificationMessage>> GetCurrentUserNotifications();

}
