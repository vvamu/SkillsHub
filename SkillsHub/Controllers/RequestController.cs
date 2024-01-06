using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Helpers;
using SkillsHub.Helpers.SearchModels;
using SkillsHub.Persistence;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Web;

namespace SkillsHub.Controllers;

[Authorize(Roles ="Admin")]
public class RequestController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;
    private readonly IRequestService _requestService;

    public RequestController(ApplicationDbContext context, IUserService userService, IRequestService requestService)
    {
        _context = context;
        _userService = userService;
        _requestService = requestService;
        //_lessonService = lessonService;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var items = await _requestService.GetAll();
        return View(items.AsQueryable());
    }



    [Authorize(Roles = "Teacher,Admin")]
    [HttpPost]
    [Route("/Request/Edit")]
    public async Task<IActionResult> RequestEdit(Lesson item)
    {
        var user = await _userService.GetCurrentUserAsync();

        var requestMessage = user.Login.ToString()
            + " want to update current lesson with values: "
            + "\nStart date : " + item.StartTime.ToShortDateString()
            + "\nEnd date : " + item.EndTime.ToShortDateString();

        var request = await _requestService.Create(item.Id, requestMessage,item);

        return RedirectToAction("Item", "Group", new { id = request.LessonBefore.GroupId });
    }
    [Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult> RequestDelete(Guid id)
    {
        var user = await _userService.GetCurrentUserAsync();
        var requestMessage = user.Login.ToString() + " want to delete current lesson";
        var request = await _requestService.Create(id,requestMessage);

        return RedirectToAction("Item", "Group", new { id = request.LessonBefore.GroupId  });
    }

    
    public async Task<IActionResult> ApplyRequest(RequestLesson item, Lesson lesson, int answer) //in button value apply or no apply
    {

        await _requestService.ApplyLessonRequest(item, lesson, answer);

        return RedirectToAction("Item", "Group", new { id = lesson.GroupId });
    }
    #region Service




    #endregion
}
