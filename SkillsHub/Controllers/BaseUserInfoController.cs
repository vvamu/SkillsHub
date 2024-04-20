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

	public BaseUserInfoController(IUserService userService, ApplicationDbContext context)
    {
        _userService = userService;
        _context = context;

	}


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
    }


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

    [HttpPost]
    public async Task<IActionResult> Next(Guid userId)
    {
        var user = await _context.ApplicationUsers.FindAsync(userId);
        if (!User.Identity.IsAuthenticated)
        {
            user = await _userService.SignInAsync(user);
        }

        return RedirectToAction("Item", "Account", new { itemId = user.Id, id = user.Id });
    }

}