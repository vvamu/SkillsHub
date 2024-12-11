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
public class BaseUserInfoController : Controller
{
    private readonly IUserService _userService;
	private readonly ApplicationDbContext _context;
    private readonly IBaseUserInfoService _baseUserInfoService;
    private readonly IApplicationUserBaseUserInfoService _applicationUserBaseUserInfoService;

    public BaseUserInfoController(IUserService userService, ApplicationDbContext context, IBaseUserInfoService baseUserInfoService
        , IApplicationUserBaseUserInfoService applicationUserBaseUserInfoService)
    {
        _userService = userService;
        _context = context;
        _baseUserInfoService = baseUserInfoService;
        _applicationUserBaseUserInfoService = applicationUserBaseUserInfoService;


    }

    /*
    public PartialViewResult AddListItem(Guid userId)
    {
        return PartialView("_Create", new BaseUserInfo () { Id = Guid.NewGuid(), ApplicationUserId = userId});
    }

    [HttpPost]
    public ActionResult DeleteListItem(Guid id)
    {
        // Логика удаления элемента из списка по id
        // Вернуть PartialView или JSON с результатом удаления
        return Json(new { success = true });
    }*/

    [HttpGet]
    public async Task<IActionResult> Edit(Guid? baseUserInfoId)
    {
        var userInfo = await _context.BaseUserInfo.FirstOrDefaultAsync(x=>x.Id == baseUserInfoId);
        var listUsers = _context.ApplicationUserBaseUserInfo.Where(x=>x.BaseUserInfoId == baseUserInfoId).ToList();
        if (userInfo == null) userInfo = new BaseUserInfo() { ApplicationUsers = listUsers };

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
    public async Task<IActionResult> Create(Guid userId,string applicationUserId)
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