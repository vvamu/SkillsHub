using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Interfaces;

public interface IUserPresentationService
{
    public PagedList<TeacherDTO> GetAllTeachers(QueryStringParameters ownerParameters);
    public Task<PagedList<StudentDTO>> GetAllStudentsAsync(QueryStringParameters ownerParameters);


}
