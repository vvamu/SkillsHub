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
public class HomeController : Controller
{
    private readonly IMailService _mailService;
    private readonly IExternalService _externalService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserService _userService;
    private readonly ILessonTypeService _lessonTypeService;
    private readonly ApplicationDbContext _context;

    public HomeController(IMailService mailService, IExternalService externalService, 
        SignInManager<ApplicationUser> signInManager, IUserService userService, ILessonTypeService lessonTypeService, ApplicationDbContext context)
    {
        _mailService = mailService;
        _externalService = externalService;
        _signInManager = signInManager;
        _userService = userService;
        _lessonTypeService = lessonTypeService;
        _context = context;
    }

    [Route("thanks")]
    public IActionResult Thanks()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "CRM");
        }
        return View();
    }

    public async Task<IActionResult> Index()
    {
        if(User.Identity.IsAuthenticated)
        {

            var user = await _userService.GetCurrentUserAsync();
            if (user.ExternalConnections != null) ViewBag.NotificationsStatus = "On - " + user.ExternalConnections.Count; else ViewBag.NotificationsStatus = "Off";
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Item", "Account", new { id = user.Id });
            }


            ViewBag.TotalTeachers = _context.Teachers.Count();
            ViewBag.TotalStudents = _context.Students.Count();
            ViewBag.ActiveTotalTeachers = _context.Teachers.Where(x => x.IsDeleted == false).Count();
            ViewBag.ActiveTotalStudents = _context.Students.Where(x => x.IsDeleted == false).Count();
            ViewBag.TotalUsers = _context.ApplicationUsers.Count();
            ViewBag.CountClasses = //_context.Teachers.Include(x => x.Lessons).Count();
            ViewBag.CountMails = _context.EmailMessages.Count();

            return View(user);
        }
        return View();
    }

    public IActionResult Test()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> Courses()
    {
        var items = await _lessonTypeService.GetAll().ToListAsync();
        var result = items.Where(x => x.ParentId == null || (x.ParentId != null && x.ParentId == Guid.Empty) && !string.IsNullOrEmpty(x.DisplayName)).ToList();
        return PartialView(result ?? new List<LessonType>());
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(SendingMessage msg)
    {
        #region ViberMessage
        /*
var _mailer = new ViberMailer.Mailer();
_mailer.Init();
await _mailer.SendMessage();
*/
        #endregion

        #region Own Viber Message

        /*
        var jsonRequest =
        "{\"receiver\":\"HieQ+DqPhvuugpuSMep9zg==\"," +
        "\"sender\":{\"name\":\"SkillsHub\"},\"type\":\"text\",\"text\":\"Hello world!\"," +
            "\"auth_token\":\"5192d0382ea7e2e9-d35f8cb4f9a26339-3a3429d4a23b0c5f\"}";
        var url = "https://chatapi.viber.com/pa/send_message";

        using (var httpClient = new HttpClient())
        {
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);
            //var res2 = await httpClient.PostAsync("https://chatapi.viber.com/pa/send_message")

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {jsonResponse}");
            }
            else
            {
                Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            }
        }
        */

        #endregion

        //if (!ModelState.IsValid) return View("Index",msg);

        await _mailService.SendEmailAsync(msg);
        var message = new SkillsHub.Domain.Models.EmailMessage() { Data = msg.Data, Email = msg.Email, Date = msg.Date, Name = msg.Name, Phone = msg.Phone };

        await _externalService.SaveMessage(message);

         return Redirect("~/thanks");
    }

    [HttpGet]
    [Route("website")]
    public async Task<IActionResult> GetWebsite()
    {
        return View("UnauthorizedIndex");
    }

    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    
}