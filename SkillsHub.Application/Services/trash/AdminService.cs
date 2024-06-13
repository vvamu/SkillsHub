using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.m;

public class AdminService
{
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public AdminService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }



}