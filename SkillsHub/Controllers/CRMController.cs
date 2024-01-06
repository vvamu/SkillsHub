using EmailProvider.Interfaces;
using EmailProvider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Helpers;
using SkillsHub.Models;
using SkillsHub.Persistence;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static Viber.Bot.NetCore.Models.ViberResponse;

namespace SkillsHub.Controllers;
[Authorize]
public class CRMController: Controller
{
    private readonly IMailService _mailService;
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _context;
    private readonly ICourcesService _courcesService;

    public  CRMController(IMailService mailService, IUserService userService,
     ApplicationDbContext context, ICourcesService courcesService)
    {
        _mailService = mailService;
        _userService = userService;
        _context = context;
        _courcesService = courcesService;
    }

    public async Task<IActionResult> Index()
    {
        //await _courcesService.InitCources();

        var user = await _userService.GetCurrentUserAsync();
        if (user.ExternalConnections != null) ViewBag.NotificationsStatus = "On - " + user.ExternalConnections.Count; else ViewBag.NotificationsStatus = "Off";
        if (User.IsInRole("Admin"))

        {
            ViewBag.TotalTeachers = _context.Teachers.Count();
            ViewBag.TotalStudents = _context.Students.Count();
            ViewBag.ActiveTotalTeachers = _context.Teachers.Where(x=>x.IsDeleted == false).Count();
            ViewBag.ActiveTotalStudents = _context.Students.Where(x => x.IsDeleted == false).Count();
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.CountClasses = //_context.Teachers.Include(x => x.Lessons).Count();
            ViewBag.CountMails = _context.EmailMessages.Count();

            return View();
        }
        else
        {
            return RedirectToAction("Item", "Account", new { itemId = user.Id });
        }

        return View(user);
    }


    public IActionResult Classes()
    {
        var events = new List<Eventh>();
        events.Add(Eventt);
        //ViewData["Events"] = JSONListHelper.GetEventListJSONString(events);
        ViewData["Events"] = "fesff";
        
        ViewBag.Events = JSONListHelper.GetEventListJSONString(events);
        

        return View();
    }

    public IActionResult Leads()
    {
        var events = new List<Eventh>();
        events.Add(Eventt);
        //ViewData["Events"] = JSONListHelper.GetEventListJSONString(events);
        ViewData["Events"] = "fesff";

        ViewBag.Events = JSONListHelper.GetEventListJSONString(events);


        return View();
    }
    public IActionResult Classes2()
    {
        var events = new List<Eventh>
        {
            Eventt
        };
        //ViewData["Events"] = JSONListHelper.GetEventListJSONString(events);
        ViewData["Events"] = "fesff";
        //ViewBag.Events = JSONListHelper.GetEventListJSONString(events);

        return View();
    }

    public JsonResult CreateNewEvent(EventDTO eventDto)
    {
        /*
        eventDto.Category = ConvertToTitleCase(eventDto.Category);
        */
        return Json("event saved");
    }

    public Eventh Eventt => new Eventh() { Name="suka", StartTime = DateTime.Now  , ClassName = "bg-primary"};

    public JsonResult GetEvents()
    {
        


        return Json("ok");
    }
    public static string ConvertToTitleCase(string input)
    {
        if (input == null) return null;
        string[] words = input.Split('-');
        return Regex.Replace(words[1], "^[a-z]", c => c.Value.ToUpper());

    }



    public JsonResult CreateNewCategory()
    {
        return Json("event saved");
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}