using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.DTO;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Interfaces;
public interface IApplicationUserBaseUserInfoService
{
    public IQueryable<ApplicationUserBaseUserInfo>? GetApplicationUserBaseUserInfo(Guid? baseUserInfoId, Guid? userId);
    public Task<ApplicationUserBaseUserInfo> CreateAsync(UserCreateDTO? user, BaseUserInfo? baseUserInfo);
    public Task<ApplicationUserBaseUserInfo> CreateAsync(Guid userId, BaseUserInfo? baseUserInfo, bool isBase = false);
    public Task<IQueryable<ApplicationUserBaseUserInfo>> UpdateAsync(UserCreateDTO? user, BaseUserInfo? baseUserInfo);
    //public Task<IQueryable<ApplicationUserBaseUserInfo>> UpdateAsync(Guid userId, BaseUserInfo? baseUserInfo);

    public Task<ApplicationUserBaseUserInfo> AddInfoToUserAsync(ApplicationUserBaseUserInfo model);
    public Task<ApplicationUserBaseUserInfo> RemoveInfoByUserAsync(Guid userId, Guid baseUserInfoId);
}
