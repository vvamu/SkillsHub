using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

public class BaseUserInfoService : IBaseUserInfoService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public BaseUserInfoService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }
    #region Get Create Update

    

    public async Task<BaseUserInfo> CreateAsync(BaseUserInfo item)
    {
        List<BaseUserInfo> children;
        #region Validators
        var userRegisterValidator = new BaseUserInfoValidator();
        var userValidationResult = await userRegisterValidator.ValidateAsync(item);
        if (!userValidationResult.IsValid)
        {
            var errors = userValidationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }
        #endregion    
        var oldValue = await _context.BaseUserInfo.FindAsync(item.Id);
        item.Id = Guid.Empty;
        var res = await _context.BaseUserInfo.AddAsync(item);

         
        //Check unique rows without considering parent rows
        var resultSearching = await _context.BaseUserInfo.Where(x => x.Parent == null).ToListAsync();
        resultSearching = resultSearching.Where(x => x.Equals(item)).ToList();
        if (oldValue != null && oldValue.Id != Guid.Empty) 
        {
            children = GetAllChildren(oldValue.Id).OrderByDescending(x => x.DateCreated).ToList();
            if (children.Count() != 0)
            {
                resultSearching = resultSearching.Where(x => !children.Select(x => x.Id).Contains(x.Id)).ToList();
                children.Add(oldValue);
                res.Entity.DateRegistration = children.LastOrDefault()?.DateCreated;
            }
        }
        if (resultSearching.Count() > 1) throw new Exception("This user already defined");
        await _context.SaveChangesAsync();
        return Get(res.Entity);
    }
    public async Task<BaseUserInfo> UpdateAsync(BaseUserInfo item)
    {
        var itemsByBaseUserInfo = _context.BaseUserInfo.FirstOrDefault(x=>x.Id == item.Id) ?? throw new Exception("No info in database");
        var resCreated = await CreateAsync(item);

        itemsByBaseUserInfo.ParentId = resCreated.Id;
        _context.BaseUserInfo.Update(itemsByBaseUserInfo);
        await _context.SaveChangesAsync();

        

        return Get(resCreated);

    }
    #endregion


    private BaseUserInfo Get(BaseUserInfo model)
    {
        
        return model;   

    }

    public IEnumerable<BaseUserInfo> GetAll()
    {
        return _context.BaseUserInfo.AsEnumerable();
    }

    #region GetAllChildren

    public IEnumerable<BaseUserInfo> GetAllChildren(Guid parentId)
    {
        var parent = _context.BaseUserInfo.FirstOrDefault(i => i.Id == parentId);

        if (parent != null)
        {
            yield return parent;
            foreach (var child in GetChildrenRecursive(parent.Id))
            {
                yield return child;
            }
        }
    }
    private IEnumerable<BaseUserInfo> GetChildrenRecursive(Guid parentId)
    {
        var children = _context.BaseUserInfo.Where(i => i.ParentId == parentId).ToList();

        foreach (var child in children)
        {
            yield return child;
            foreach (var grandChild in GetChildrenRecursive(child.Id))
            {
                yield return grandChild;
            }
        }
    }


    #endregion

    #region GetAllParents

    public IEnumerable<BaseUserInfo> GetAllParents(Guid childId)
    {
        var child = _context.BaseUserInfo.FirstOrDefault(i => i.Id == childId);

        if (child != null)
        {
            yield return child;
            foreach (var parent in GetParentsRecursive((Guid)child.ParentId))
            {
                yield return parent;
            }
        }
    }

    private IEnumerable<BaseUserInfo> GetParentsRecursive(Guid childParentId)
    {
        var parent = _context.BaseUserInfo.FirstOrDefault(i => i.Id == childParentId);

        if (parent != null)
        {
            yield return parent;
            foreach (var grandParent in GetParentsRecursive((Guid)parent.ParentId))
            {
                yield return grandParent;
            }
        }
    }

    #endregion
}