using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.DTO;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Interfaces;

public interface ISalaryService
{
    public Task<decimal> GetStudentSalaryAsync(Student student, bool withNotCompletedLessons = false, bool byMonth = false);
    public Task<decimal> GetTeacherSalaryAsync(Teacher teacher, bool withNotCompletedLessons = false, bool byMonth = false);

}
