using EmailProvider.Interfaces;
using EmailProvider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Implementation.User;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Persistence;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkillsHub.Controllers;
public class HomeController : Controller
{
    private readonly IMailService _mailService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserService _userService;
    private readonly ILessonTypeService _lessonTypeService;
    private readonly ApplicationDbContext _context;

    public HomeController(IMailService mailService,
        SignInManager<ApplicationUser> signInManager, IUserService userService, ILessonTypeService lessonTypeService, ApplicationDbContext context)
    {
        _mailService = mailService;
        _signInManager = signInManager;
        _userService = userService;
        _lessonTypeService = lessonTypeService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity.IsAuthenticated)
        {

            var user = await _userService.GetCurrentUserAsync();
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Item", "Account", new { id = user.Id });
            }

            ViewBag.TotalTeachers = _context.Teachers.Count();
            ViewBag.TotalStudents = _context.Students.Count();
            ViewBag.ActiveTotalTeachers = _context.Teachers.Where(x => x.IsDeleted == false).Count();
            ViewBag.ActiveTotalStudents = _context.Students.Where(x => x.IsDeleted == false).Count();
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.CountClasses = //_context.Teachers.Include(x => x.Lessons).Count();
            ViewBag.CountMails = _context.EmailMessages.Count();

            return View("IndexCRM", user);
        }
        return View("~/Views/Home/Index/Index.cshtml");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new Helpers.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    #region Authorized

    [HttpGet]
    public async Task<IActionResult> Courses()
    {
        var items = await _lessonTypeService.GetAll().ToListAsync();
        var result = items.Where(x => x.ParentId == null || (x.ParentId != null && x.ParentId == Guid.Empty) && !string.IsNullOrEmpty(x.DisplayName)).ToList();
        return PartialView("./Views/Home/Index/Courses/Courses.cshtml", result ?? new List<LessonType>());
    }

    #region ChangeMainPage

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetCoursesToSelect()
    {
        var notifications = await _userService.GetCurrentUserNotifications();
        return PartialView("_Chat", notifications.OrderByDescending(x => x.DateCreated).ToList());
     
    }


    public async Task<string> GetEditorHtml(string html,string? id)
    {
        if (string.IsNullOrEmpty(html)) return "";
        var res = html;
        

        string substringToFind = ">save</button></div>";
        int index = res.IndexOf(substringToFind);

        if (index != -1)
        {
            string result = res.Substring(index + substringToFind.Length);
        }
        var textAreaId = "textArea" + id;
        textAreaId = textAreaId.Replace("'", "");
        id = '"' + id;
        id = id + '"';
        

        res = res.Replace("<br><br>", "\n----\n").Replace("<br> <br>-", "\n- \n-").Replace("<br>", "\n").Replace("<hr>", "\n--\n");//.Replace("<br/> -", "- -").Replace("<br/>", "\n").Replace("<hr/>", "--");
        var htm = $"<button onclick='getResult({id})'>change to result view</button> <br/><textarea class='textareaDesr' type='text' id='{textAreaId}'>{res}</textarea>";
        var val = new Microsoft.AspNetCore.Html.HtmlString(htm);

        return val.Value;
    }
    public async Task<string> ToHtml(string? courseId, string editorValue)
    {
        if (string.IsNullOrEmpty(editorValue)) return "";

        var firstSymbol = editorValue.Substring(0, 2).ToString();
        var str = editorValue.Substring(2).ToString().Replace("\n", "").Replace("----","<br><br>").Replace("--", "<hr/>").Replace("- -", "<br/> -").Replace("-", "<br/>-").Replace("<hr/><br/>", "<hr/>").Replace("\n","<br/>");
        var val = new Microsoft.AspNetCore.Html.HtmlString(firstSymbol + str);
        return val.Value;
    }
    [HttpPost]
    public async Task<IActionResult> SaveDescriptionOnMainPage(string courseId, string html)
    {
        try
        {
            Guid courseID;
            if (!Guid.TryParse(courseId, out courseID)) throw new Exception("Not correct course id");
            var course = await _context.Courses.FindAsync(courseID);
            if (course is null) throw new Exception("Not found course with such id");
            if (string.IsNullOrEmpty(html)) throw new Exception("not passed html");

            var res = html.Replace("<button onclick=\"getEditor()\">change to editor view</button>", "");
            res = res.Replace("<div style=\"display:flex\"><button onclick=\"saveChanges()\">save</button></div>", "");
            res = res.Replace("<br><br>", "\n----\n").Replace("<br> <br>-", "\n- \n-").Replace("<br>", "\n").Replace("<hr>", "\n--\n");
            var str = res.Substring(1).ToString();
            course.DescriptionOnMainPage = str;
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {

            return BadRequest(new { error = ex.Message });
        }

        return Json("Compete successfully");
    }

    #endregion

    [HttpGet]
    [Route("website")]
    public async Task<IActionResult> GetWebsite()
    {
        return View("./Views/Home/Index/Index.cshtml");
    }

    [HttpGet]
    public async Task<IActionResult> InstitutionSetting()
    {
        return View("InstitutionSettingPage");
    }


    #endregion

    #region Not Authorized

    [HttpPost]
    public async Task<IActionResult> SendMessage(SendingMessage msg)
    {
        await _mailService.SendEmailAsync(msg);
        var message = new SkillsHub.Domain.Models.EmailMessage() { Data = msg.Data, Email = msg.Email, Date = msg.Date, Name = msg.Name, Phone = msg.Phone };

        //await _mailService.SaveMessage(message);

        return Redirect("~/thanks");
    }

    public IActionResult Test()
    {
        return View();
    }

    [Route("thanks")]
    public IActionResult Thanks()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index");
        }
        return View("./Views/Home/Index/Thanks.cshtml");
    }

    #endregion

}