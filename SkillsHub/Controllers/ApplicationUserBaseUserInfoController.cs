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
using SkillsHub.Domain.Models;
using SkillsHub.Helpers;
using SkillsHub.Models;
using SkillsHub.Persistence;
using System.Diagnostics;
using System.Text;
using static Viber.Bot.NetCore.Models.ViberResponse;

namespace SkillsHub.Controllers;
public class ApplicationUserBaseUserInfoController : Controller
{
    private readonly IUserService _userService;
	private readonly ApplicationDbContext _context;
    private readonly IBaseUserInfoService _baseUserInfoService;
    private readonly IApplicationUserBaseUserInfoService _applicationUserBaseUserInfoService;

    public ApplicationUserBaseUserInfoController(IUserService userService, ApplicationDbContext context, IBaseUserInfoService baseUserInfoService
        , IApplicationUserBaseUserInfoService applicationUserBaseUserInfoService)
    {
        _userService = userService;
        _context = context;
        _baseUserInfoService = baseUserInfoService;
        _applicationUserBaseUserInfoService = applicationUserBaseUserInfoService;


    }

    [HttpGet]
    [Authorize("Admin")]
    public async Task<IActionResult> Index()
    {
        var items = await _applicationUserBaseUserInfoService.GetApplicationUserBaseUserInfo(null, null)?.Where(x=>x.BaseUserInfo.ParentId == Guid.Empty).ToListAsync();
      
        return View("~/Views/BaseUserInfo/Index.cshtml", items);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid userId, Guid? baseUserInfoId)
    {
        var userInfo = await _context.BaseUserInfo.FirstOrDefaultAsync(x=>x.Id == baseUserInfoId);
        var listUsers = new List<ApplicationUserBaseUserInfo>() { new ApplicationUserBaseUserInfo() { ApplicationUserId = userId } };
        if (userInfo == null) userInfo = new BaseUserInfo() { ApplicationUsers = listUsers, ApplicationUserId =userId };

        return View("Create",userInfo);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(BaseUserInfo model)
    {
        try
        {
            var applicationUserBaseUserInfo = await _applicationUserBaseUserInfoService.UpdateAsync(null, model);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(); }

        return RedirectToAction("Item", new { itemId = model.ApplicationUserId });
    }

    [HttpGet]
    public async Task<IActionResult> AddToUser(ApplicationUserBaseUserInfo model)
    {
        try
        {
            var applicationUserBaseUserInfo = await _applicationUserBaseUserInfoService.AddInfoToUserAsync(model);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(); }

        return RedirectToAction("Item", new { itemId = model.ApplicationUserId });
    }



    [HttpPost]
    public async Task<IActionResult> RemoveFromUser(Guid userId, Guid baseUserInfoId)
    {
        try
        {
            var applicationUserBaseUserInfo = await _applicationUserBaseUserInfoService.RemoveInfoByUserAsync(userId, baseUserInfoId);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(); }

        return RedirectToAction("Item", new { itemId = userId });
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid userId)
    {
        var userInfo = new BaseUserInfo() { ApplicationUserId = userId };
        return View(userInfo);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BaseUserInfo model)
    {
        BaseUserInfo userInfo;
        try
        {
            var applicationUserBaseUserInfo = await _applicationUserBaseUserInfoService.CreateAsync(null, model);
            userInfo = applicationUserBaseUserInfo.BaseUserInfo;

        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(); }
        


        return RedirectToAction("Item", new { itemId = model.ApplicationUserId });
    }

    [HttpPost]
    public async Task<IActionResult> Next(Guid userId, bool complete = true)
    {
        /*
        var user = await _context.ApplicationUsers.FindAsync(userId);
        if (!User.Identity.IsAuthenticated)
        {
            user = await _userService.SignInAsync(user);
        }

        return RedirectToAction("Item", "Account", new { itemId = user.Id, id = user.Id });*/
        if (complete) return RedirectToAction("SignIn", "Account");
        else return RedirectToAction("Create", userId);

    }


    #region Ajax - get


    /*
    [HttpGet]
    public ActionResult Create(Guid userId)
    {

        return PartialView("Index", userId);

    }

    [HttpPost]
    public ActionResult Create(BaseUserInfo model)
    {

        return Json(new { success = true });

    }
    */
    #endregion
}