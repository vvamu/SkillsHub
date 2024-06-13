using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using System.Data;

namespace SkillsHub.Application.Services.Implementation;

public class ApplicationUserBaseUserInfoService : IApplicationUserBaseUserInfoService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IBaseUserInfoService _baseUserInfoService;
    private readonly IUserService _userService;

    public ApplicationUserBaseUserInfoService(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
        IMapper mapper, IBaseUserInfoService baseUserInfoService,IUserService userService)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
        _baseUserInfoService = baseUserInfoService;
        _userService = userService;
    }
    public async Task<ApplicationUserBaseUserInfo> CreateAsync(UserCreateDTO? user,BaseUserInfo? baseUserInfo)
    {
        BaseUserInfo userInfoDb;
        var userInfo = _mapper.Map<BaseUserInfo>(user);
        ApplicationUser userDb = await _context.ApplicationUsers.FindAsync(userInfo.ApplicationUserId);
         
        userDb = await _userService.CreateUserAsync(user);
                
        return await CreateAsync(userDb.Id, userInfo, user.IsBase);

    }
    public async Task<ApplicationUserBaseUserInfo> CreateAsync(Guid userId, BaseUserInfo? baseUserInfo,bool isBase = false)
    {
        BaseUserInfo userInfoDb;
        if (baseUserInfo == null) userInfoDb = await _baseUserInfoService.CreateAsync(baseUserInfo);
        else userInfoDb = await _baseUserInfoService.CreateAsync(baseUserInfo);

        var newApplicationBaseUserInfo = new ApplicationUserBaseUserInfo() { ApplicationUserId = userId, BaseUserInfoId = userInfoDb.Id, IsBase = isBase, Role = baseUserInfo.Role };
        var dbRes = await _context.ApplicationUserBaseUserInfo.AddAsync(newApplicationBaseUserInfo);
        await _context.SaveChangesAsync();

        var rese = GetApplicationUserBaseUserInfo(dbRes.Entity.BaseUserInfoId, dbRes.Entity.ApplicationUserId);
        var rese2 = await rese.ToListAsync();
        return await rese.FirstOrDefaultAsync();
    }
    public async Task<ApplicationUserBaseUserInfo> AddInfoToUserAsync(ApplicationUserBaseUserInfo model)
    {
        if (_context.ApplicationUsers.Find(model.ApplicationUserId) == null || _context.BaseUserInfo.Find(model.BaseUserInfoId) == null) throw new Exception("User is not found");
        var dbRes = await _context.ApplicationUserBaseUserInfo.AddAsync(model);
        await _context.SaveChangesAsync();
        return dbRes.Entity;
    }
    public async Task<ApplicationUserBaseUserInfo> RemoveInfoByUserAsync(Guid userId, Guid baseUserInfoId)
    {
        if (_context.ApplicationUsers.Find(userId) == null || _context.BaseUserInfo.Find(baseUserInfoId) == null) throw new Exception("User is not found");
        var oldValue = _context.ApplicationUserBaseUserInfo.FirstOrDefault(x => x.ApplicationUserId == userId && x.BaseUserInfoId == baseUserInfoId) ?? throw new Exception("Connection between user and user info not found");
        var dbRes = _context.ApplicationUserBaseUserInfo.Remove(oldValue);
        await _context.SaveChangesAsync();
        return dbRes.Entity;
    }


    //Метод для полного обновления пользователя (user + userInfo)
    //Надо чтобы только у нового было ParentId = null
    //Надо чтобы у элемента перед было ParentId = newParentId
    //Надо удалить все связи в ApplicationUserBaseUserInfo с прошлым ParentId, добавить новые 
    public async Task<IQueryable<ApplicationUserBaseUserInfo>?> UpdateAsync(UserCreateDTO? user, BaseUserInfo? item)
    {
        ApplicationUser userDb;
        BaseUserInfo newUserInfo = item;
        BaseUserInfo? dbUserInfo = null;
        
        if(user != null)
        {
            userDb = await _userService.UpdateUserAsync(user);
            dbUserInfo = userDb?.UserInfo;
            newUserInfo = _mapper.Map<BaseUserInfo>(user);
            item = newUserInfo;
            item.Id = user.BaseUserInfoId;
        }
        if (item != null)
        {
            dbUserInfo = await _context.BaseUserInfo.FindAsync(item.Id);
            var itemsByBaseUserInfo = GetApplicationUserBaseUserInfo(user.BaseUserInfoId, Guid.Empty)?.Select(x => x.BaseUserInfo) ?? throw new Exception("No info in database");
            if (itemsByBaseUserInfo.Count() == 0) throw new Exception("No info of connected account in database");

            if (dbUserInfo != null && AreBaseUserInfoDifferent(dbUserInfo,newUserInfo))
            {
                dbUserInfo = newUserInfo;
                dbUserInfo.Id = user.BaseUserInfoId;

                var updatedUserInfo = await _baseUserInfoService.UpdateAsync(dbUserInfo);

                var itemsToUpdate = _context.ApplicationUserBaseUserInfo.Where(x => x.BaseUserInfoId == user.BaseUserInfoId); //поменялся baseUserInfo -> появился новый элемент -> все связи надо обновить на новые
                foreach (var i in itemsToUpdate)
                {
                    i.BaseUserInfoId = updatedUserInfo.Id;
                    _context.ApplicationUserBaseUserInfo.Update(i);
                    await _context.SaveChangesAsync();
                }
            }

            await _context.SaveChangesAsync();
        }

        return GetApplicationUserBaseUserInfo(Guid.Empty, user.Id);

    }
    public IQueryable<ApplicationUserBaseUserInfo>? GetApplicationUserBaseUserInfo(Guid? baseUserInfoId, Guid? userId)
    {
        var items = _context.ApplicationUserBaseUserInfo
            .Include(x => x.ApplicationUser)
            .Include(x => x.BaseUserInfo).AsQueryable();
        if (userId != Guid.Empty)
            items = items.Where(x => x.ApplicationUserId == userId);
        if (baseUserInfoId != Guid.Empty)
            items = items.Where(x => x.BaseUserInfoId == baseUserInfoId);

        return items;
    }

    protected bool AreBaseUserInfoDifferent(BaseUserInfo oldValue, BaseUserInfo newItem)
    {
        var properties = typeof(BaseUserInfo).GetProperties();
        foreach (var property in properties)
        {
            var oldValuePropValue = property.GetValue(oldValue);
            var newItemPropValue = property.GetValue(newItem);

            if (oldValuePropValue == null && newItemPropValue == null)
            {
                continue; // Both values are null, move to the next property
            }

            if ((oldValuePropValue == null && newItemPropValue != null) || (oldValuePropValue != null && newItemPropValue == null) || !oldValuePropValue.Equals(newItemPropValue))
            {
                return true;
            }
        }

        return false;
    }

}