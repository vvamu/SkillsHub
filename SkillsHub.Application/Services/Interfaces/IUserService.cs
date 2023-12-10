using SkillsHub.Application.DTO;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Interfaces;

public interface IUserService
{
    public Task InitialCreateAsync();
    public Task<Teacher> CreateTeacherAsync(Guid userId, Teacher item);
    public Task<Student> CreateStudentAsync(Guid userId, Student item);
    public Task<ApplicationUser> CreateUserAsync(UserCreateDTO user);
    public Task<ApplicationUser?> GetCurrentUserAsync();
    public Task<ApplicationUser> SignInAsync(UserLoginDTO item);
    public Task<ApplicationUser> SignInAsync(ApplicationUser item);
    public Task SignOutAsync();
    public Task<ApplicationUser> GetUserByIdAsync(Guid id);
    public Task<bool> IsAdminAsync();

    public Task<IQueryable<ApplicationUser>> GetAllAsync();

    public Task<IQueryable<StudentDTO>> GetAllStudentsAsync();

    public Task<IQueryable<Teacher>> GetTeachersByLessonTypeAsync(Guid lessonTypeId);

    public Task<Teacher> CreatePossibleCourcesNamesToTeacherAsync(Guid itemId, List<Guid> courcesId);
    public Task<Student> CreatePossibleCourcesNamesToStudentAsync(Guid itemId, List<Guid> courcesId);


    public IQueryable<TeacherDTO> GetAllTeachers();


}
