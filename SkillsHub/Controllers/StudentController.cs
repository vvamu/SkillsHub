using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Helpers;
using SkillsHub.Persistence;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SkillsHub.Controllers;

public class StudentController : Controller
{
    private readonly IUserPresentationService _userPresentationService;
    private readonly ICourcesService _courcesService;
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _context;

    public StudentController(IUserPresentationService userPresentationService, ICourcesService courcesService,
        IUserService userService,ApplicationDbContext context)
    {
        _userPresentationService = userPresentationService;
        _courcesService = courcesService;
        _userService = userService;
        _context = context;
        //var items = _userService.GetAllStudentsAsync().Result;
        //Reporter.ReportToXLS<Student>(items.ToArray());
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = _context.ApplicationUsers.Include(x=>x.UserStudent).Where(x => x.UserStudent != null);
        var students = _context.Students.AsQueryable();

        return View((users,students));
    }

    [HttpGet]
    [Route("/Student/GetStudentListAsync")]
    public async Task<IActionResult> GetStudentListAsync()
    {
        var items = await _userService.GetAllStudentsAsync();
        return Json(JsonSerializerToAjax.GetJsonByIQueriable(items));
    }

    [HttpGet]
    public IActionResult Edit()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Delete()
    {
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Create(Guid id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id) ?? await _userService.GetCurrentUserAsync();            
            var student = _context.Students.Include(x=>x.ApplicationUser).FirstOrDefault(x=>x.ApplicationUser.Id == user.Id) ?? new Student();
            student.ApplicationUserId = user.Id;

            return View(student);
        }
        catch (Exception)
        {
            var a = HttpContext.Request.PathBase;
            var user = await _userService.GetCurrentUserAsync();

            if (user == null) return View("Index");
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Student item, Guid[] itemValue)
    {
        var student = _context.Students.Include(x => x.ApplicationUser).FirstOrDefault(x => x.ApplicationUser.Id == item.ApplicationUserId);
        if(student == null)
        student = await _userService.CreateStudentAsync(item.Id, item);
        else
        {
            _context.Students.Update(student);

        }
        await _userService.CreatePossibleCourcesNamesToStudentAsync(student.Id, itemValue.ToList());

        if (await _userService.IsAdminAsync()) return RedirectToAction("Index", "CRM");
        var user = student.ApplicationUser;

        var userDb = await _userService.SignInAsync(user);
        if (userDb == null) return View(userDb);
        return RedirectToAction("Index", "CRM");
    }
}
