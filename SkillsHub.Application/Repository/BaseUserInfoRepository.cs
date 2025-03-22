using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Repository;

public class BaseUserInfoRepository : AbstractLogModelService<BaseUserInfo>, IAbstractLogModelService<BaseUserInfo>
{
    protected override DbSet<BaseUserInfo> _contextModel => _context.BaseUserInfo;

    protected override FluentValidation.IValidator<BaseUserInfo> _validator => new BaseUserInfoValidator();

    protected override IQueryable<BaseUserInfo>? _fullInclude => _context.BaseUserInfo;

    public BaseUserInfoRepository(ApplicationDbContext context) : base(context) { }
}