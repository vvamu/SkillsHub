using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Helpers;
using SkillsHub.Helpers.SearchModels;
using SkillsHub.Persistence;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Web;
using static NuGet.Packaging.PackagingConstants;

namespace SkillsHub.Controllers;

public class StudentController : Controller
{
    private readonly ICourcesService _courcesService;
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGroupService _groupService;

    public StudentController(ICourcesService courcesService,
        IUserService userService,ApplicationDbContext context, UserManager<ApplicationUser> userManager, IGroupService groupService)
    {
        _courcesService = courcesService;
        _userService = userService;
        _context = context;
        _userManager = userManager;
        _groupService = groupService;
        //var items = _userService.GetAllStudentsDTOAsync().Result;
        //Reporter.ReportToXLS<Student>(items.ToArray());
    }
    #region NotUse

    [HttpGet]
    public async Task<IActionResult> Index2()
    {
        var users = _context.ApplicationUsers.Include(x => x.UserStudent).Where(x => x.UserStudent != null);
        var students = _context.Students.AsQueryable();


        return View((users, students));
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllStudentsAsync();


        return View(users);
    }

    #endregion

    #region Get

    [HttpGet]
    [Route("/Student/GetStudentListAsync")]
    public async Task<IActionResult> GetStudentListAsync([FromQuery] StudentFilterModel StudentFilterModel, StudentOrderModel? orders)
    {


        var items = _context.Students.Include(x => x.ApplicationUser)
            .Where(x => x.IsDeleted == false)
            .ToList().AsQueryable();

        if (!User.IsInRole("Admin"))
        {
            var user = await _userService.GetCurrentUserAsync();
            var userStudent = user.UserStudent ?? new Student();
            var usersByUserGroups = _context.Groups.Include(x => x.GroupStudents).
                SelectMany(x=>x.GroupStudents).
                Select(x=>x.Student).
                Where(x => x.Id ==  userStudent.Id).
                //Select(x=>x.ApplicationUser).
                Where(x => x.IsDeleted == false); ;

            // items = usersByUserGroups;
        }
        try
        {
            var jsonString = HttpContext.Request.QueryString.Value ?? "";
            jsonString = jsonString.Substring(1); // Remove '?'
            jsonString = HttpUtility.UrlDecode(jsonString);

            // Deserialize the JSON string
            var jsonObject = JObject.Parse(jsonString);
            var filterModel = jsonObject["StudentFilterModel"].ToObject<StudentFilterModel>();
            //items = await FilterMaster.GetAllStudents(items, filterModel, orders);

        }
        catch (Exception ex) { }


        //items = await FilterMaster.GetAllStudents(items, filters, orders);

        //HttpContext.Request.QueryString("unknownQuerystring").ToString()
        var ii = items.ToList();

        return Json(JsonSerializerToAjax.GetJsonByIQueriable(items));
    }

    

    [HttpPost]
    [Route("/Student/StudentsCheckedCheckboxListByObject")]
    public async Task<IActionResult> StudentsCheckedCheckboxListByObject(Guid groupId, Guid lessonId)
    {
        List<Student> selectedStudents = new List<Student>();
        Group group = await _groupService.GetAsync(groupId);
        Lesson lesson = await _context.Lessons.Include(x=>x.ArrivedStudents).FirstOrDefaultAsync(x=>x.Id == lessonId);

        var items = await _userService.GetAllStudentsAsync();
        items = items.Where(x => x.IsDeleted == false);

        //if (group == null && lesson == null) return null;
        
        if(group != null)
        {
            selectedStudents = group.GroupStudents.Select(x => x.Student).ToList(); //studentsByGroup
        }
        if(lesson != null)
        {
            group = await _context.Groups.Include(x => x.Lessons).Include(x=>x.GroupStudents).FirstOrDefaultAsync(x => x.Lessons.Select(x => x.Id).Contains(lessonId));
            if (group == null) return null;
            items = group.GroupStudents.Select(x => x.Student).AsQueryable();
            selectedStudents = lesson.ArrivedStudents.Select(x=>x.Student).ToList();
        }



        
        //--------------------------------------------------------------------------
        if (!User.IsInRole("Admin"))
        {
            var user = await _userService.GetCurrentUserAsync();
            var userStudent = user.UserStudent ?? new Student();
            var userTeacher = user.UserTeacher ?? new Teacher();

            if (userTeacher != null && user.UserTeacher.Groups != null)
            {
               // user.UserTeacher.Groups//.SelectMany(x => x.GroupStudents).Select(x => x.Student)
                   // .ToList().ForEach(x => students.Add(x));
            }
            if(userStudent !=null)
            {
                //var studentGroups = user.UserStudent.Groups.Select(x => x.Group);
                //studentGroups.SelectMany(x=>x.GroupStudents).Select(x=>x.Student).ToList().ForEach(x=>students.Add(x));
                //studentGroups.Select(x => x.Teacher).ToList().ForEach(x=>st);
            }
            // items = usersByUserGroups;
        }
        //--------------------------------------------------------------------------
      
        var all = items.ToList();          


        return PartialView("_StudentsCheckedCheckboxListByObject",(all, selectedStudents));
    }

    #endregion
    #region Create = Update

    [HttpGet]
    public async Task<IActionResult> Create(Guid id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id) ?? await _userService.GetCurrentUserAsync();
            var student = _context.Students
                .Include(x => x.PossibleCources)
                .FirstOrDefault(x => x.ApplicationUserId == user.Id) ?? new Student();

            var possibleCourses = await _context.Students.Where(x => x.ApplicationUserId == user.Id).SelectMany(x => x.PossibleCources).ToArrayAsync();
            ViewBag.PossibleCourses = possibleCourses;


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
    public async Task<IActionResult> Create(Student item, Guid[] itemValue, string[] workingDay)
    {//itemValue = students
        var student = _context.Students.AsNoTracking().Include(x => x.ApplicationUser).FirstOrDefault(x => x.Id == item.Id);

        ApplicationUser user = new ApplicationUser();//= student.ApplicationUser  ?? new ;
        if(student != null) { user = student.ApplicationUser; }

        try
        {
            
            if (student == null)
            {
                user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == item.ApplicationUserId);

                student = await _userService.CreateStudentAsync(user, item);
                item = student;
            }


            item = await UpdateWorkingDays(item, workingDay);

            item = await _userService.UpdateStudentWithCourcesNames(item, itemValue.ToList());
            //var resItem = await CreateScheduleDaysToStudentWihoutSaving(item, dayName, startTime,duration);
            await _context.SaveChangesAsync();


        }
        catch { }
        //var user = await _userService.GetUserByIdAsync(item.ApplicationUserId);

        var isTeacher = HttpContext.Session.GetString("isTeacher");
        if (isTeacher == "true")
            return RedirectToAction("Create", "Teachers", new { id = user.Id });



        if (!User.Identity.IsAuthenticated)
        {
            user = await _userService.SignInAsync(user);
        }

        return RedirectToAction("Item", "Account", new { itemId = item.ApplicationUserId, id = item.ApplicationUserId });
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


    public async Task<IActionResult> Activate(Guid id, Guid userId)
    {
        var isUser = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
        if(isUser != null)
         return RedirectToAction("Create", "Student", new { id = id });

        var item = await _context.Students
            .Include(x => x.ApplicationUser)
            .FirstOrDefaultAsync(x => x.Id == id) ?? new Student() { ApplicationUserId = id };
       
        if (await _userManager.IsInRoleAsync(item.ApplicationUser, "Student"))
        {
            item.IsDeleted = true;
            await _userManager.RemoveFromRoleAsync(item.ApplicationUser, "Student");
        }
        else
        {
            item.IsDeleted = false;
            await _userManager.AddToRoleAsync(item.ApplicationUser, "Student");
        }

            _context.Students.Update(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Item", "Account", new { itemId = item.ApplicationUserId, id = item.ApplicationUserId });

        }
}
