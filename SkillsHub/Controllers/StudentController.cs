using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Persistence;

namespace SkillsHub.Controllers;

[Authorize]
public class StudentController : Controller
{
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGroupService _groupService;
    private readonly INotificationService _notificationService;
    private readonly IUserRoleModelService<Student> _studentService;

    public StudentController(
        IUserService userService,ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
        IGroupService groupService, INotificationService notificationService, IUserRoleModelService<Student> studentService)
    {
        _userService = userService;
        _context = context;
        _userManager = userManager;
        _groupService = groupService;
        _notificationService = notificationService;
        _studentService = studentService;
        //var items = _userService.GetAllStudentsDTOAsync().Result;
        //Reporter.ReportToXLS<Student>(items.ToArray());
    }

    #region Create = Update =  Get

    [HttpGet]
    public async Task<IActionResult> Create(Guid id, bool isEdit = false)
    {
        try
        {
            Student student = new Student();
            var user = await _context.ApplicationUsers.FindAsync(id) ?? await _userService.GetCurrentUserAsync();
            student = await _context.Students.FirstOrDefaultAsync(x => x.ApplicationUserId == id);
            if (student == null) student = await _studentService.CreateAsync(new Student() { ApplicationUserId = user.Id }, new Guid[0]);
            else student = await _studentService.GetAsync(student.Id);
            
            HttpContext.Session.SetInt32("isEdit", Convert.ToInt16(isEdit) );
            return View(student);
        }
        catch (Exception)
        {
            var a = HttpContext.Request.PathBase;
            var user = await _userService.GetCurrentUserAsync();

            if (user == null) return View("Index");
            return View(new Student() { ApplicationUserId = id });


        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Student item, Guid[] lessonTypeId) // string[]? workingDay , itemValue = courceName
    {
        var student = await _context.Students.FindAsync(item.Id);
        ApplicationUser user = await _context.ApplicationUsers.FindAsync(item.ApplicationUserId);
        //if (user == null) { ModelState.AddModelError("", "User not found"); return View(item); }
        try
        {
            if (student == null)
            {
                student = await _studentService.CreateAsync(item, lessonTypeId);
                //item = student;
                //user.UserStudent = item;
            }
            else
            {
                student = await _studentService.UpdateAsync(item, lessonTypeId);
            }
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(item); }

        var isTeacher = HttpContext.Session.GetString("isTeacher");
        HttpContext.Session.Remove("isEdit");
        if (isTeacher == "true")
        {
            
            return RedirectToAction("Create", "Teachers", new { id = user.Id });
        }   
        else if (HttpContext.Session.GetInt32("isEdit") == 0)
        {
            return RedirectToAction("Create", "BaseUserInfo", new { userId = user.Id });
        }
        else
        {
            return RedirectToAction("Item", "Account", new { userId = user.Id , id = user.Id});
        }



        
    }

    [Route("/Student/GetAsync")]
    public async Task<IActionResult> GetAsync(Guid? lessonTypeId)
    {
        var items = await _studentService.GetAllAsync();
        if (lessonTypeId != null && lessonTypeId != Guid.Empty)
        {
            var result = items.ToList();
            var oks = result.Where(x => x.CurrentPossibleCourses != null && x.CurrentPossibleCourses.Select(x => x.LessonTypeId).ToList().Contains((Guid)lessonTypeId)).ToList();
            return Ok(oks);
        }
        return Ok(await items.ToListAsync());
    }
    #endregion

    #region WorkingDays/Schedule

    public async Task<Student> UpdateWorkingDays(Student item, string[] workingDay) //string[] dayName, TimeSpan[] startTime , int[] duration
    {
        item.WorkingDays = String.Join("-", workingDay);
        _context.Students.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    #endregion
}
