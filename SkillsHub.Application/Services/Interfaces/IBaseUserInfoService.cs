using Microsoft.AspNetCore.Mvc;
using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Application.Services.Interfaces;
public interface IBaseUserInfoService
{
    public Task<BaseUserInfo> CreateAsync(BaseUserInfo item);
    public Task<BaseUserInfo> UpdateAsync(BaseUserInfo item);
    public IEnumerable<BaseUserInfo> GetAllChildren(Guid parentId);
    public IEnumerable<BaseUserInfo> GetAllParents(Guid childId);



}
