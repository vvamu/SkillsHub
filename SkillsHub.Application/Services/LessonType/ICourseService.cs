using SkillsHub.Application.Repository.Base;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Implementation;

public interface ICourseService : IAbstractLogModelService<Course>, IUploadIconService<Course>, IUploadImageService<Course>
{
}