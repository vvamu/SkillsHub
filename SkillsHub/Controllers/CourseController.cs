using AutoMapper;
using EmailProvider.Interfaces;
using EmailProvider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Helpers;
using SkillsHub.Models;
using SkillsHub.Persistence;
using System.Diagnostics;
using System.Text;
using static Viber.Bot.NetCore.Models.ViberResponse;

namespace SkillsHub.Controllers;
public class CourseController : Controller
{
    private readonly IMailService _mailService;
    private readonly IExternalService _externalService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserService _userService;
	private readonly ApplicationDbContext _context;

	public CourseController(IMailService mailService, IExternalService externalService, 
        SignInManager<ApplicationUser> signInManager, IUserService userService, ApplicationDbContext context)
    {
        _mailService = mailService;
        _externalService = externalService;
        _signInManager = signInManager;
        _userService = userService;
        _context = context;

	}

    [HttpPost]
    public async Task<IActionResult> OptionsList(string subjectId,string courseId)
    {
        var courses = _context.Courses.AsAsyncQueryable();
        var subjId = Guid.TryParse(subjectId?.ToString(), out var result) ? result : Guid.Empty;
        var coursId = Guid.TryParse(courseId?.ToString(), out var resultC) ? resultC : Guid.Empty;

       // if (subjId != Guid.Empty) courses = courses.Where(x => x.SubjectId == subjId);

        return PartialView((courses, coursId));    
    }

}