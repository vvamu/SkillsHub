using AutoMapper;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Services;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Helpers;
using SkillsHub.Helpers.SearchModels;
using SkillsHub.Persistence;
using static NuGet.Packaging.PackagingConstants;

namespace SkillsHub.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IGroupService _groupService;
    private readonly ISalaryService _salaryService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILessonService _lessonService;

    public AccountController(IUserService userService,
        ApplicationDbContext context, IMapper mapper, IGroupService groupService, ISalaryService salaryService
        ,UserManager<ApplicationUser> userManager, ILessonService lessonService)
    {
        _userService = userService;
        _context = context;
        _mapper = mapper;
        _groupService = groupService;
        _salaryService = salaryService;
        _userManager = userManager;
        _lessonService = lessonService;
    }
    #region Get

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        //var users = await _userService.GetAllAsync();
        HttpContext.Session.SetString("page", "index");
        return View();

    }
    [HttpGet]
    public async Task<IActionResult> Item(Guid itemId, Guid id)
    {
        ApplicationUser? user;
        if (itemId == Guid.Empty) itemId = id;

        user = await _userService.GetUserByIdAsync(itemId);
        if (user == null) user = await _userService.GetCurrentUserAsync();
        HttpContext.Session.SetString("page", "item");

        if (user.UserTeacher != null)
        {
            user.UserTeacher.CurrentCalculatedPrice = await _salaryService.GetTeacherSalaryAsync(user.UserTeacher);
            user.UserTeacher.TotalCalculatedPrice = await _salaryService.GetTeacherSalaryAsync(user.UserTeacher, true);
        }
        if(user.UserStudent != null)
        {
            user.UserStudent.CurrentCalculatedPrice = await _salaryService.GetStudentSalaryAsync(user.UserStudent);
            user.UserStudent.TotalCalculatedPrice = await _salaryService.GetStudentSalaryAsync(user.UserStudent,true);
        }


        return View(user);
    }

    [HttpPost]
    [Route("/Account/UsersTableList")]

    public async Task<IActionResult> UsersTableList(UserFilterModel filters, OrderModel order)
    {
        var users = await _userService.GetAllAsync();
        users = await FilterMaster.FilterUsers(users, filters,order);
        List<ApplicationUser> list = new List<ApplicationUser>();

        if(!string.IsNullOrEmpty(filters.UserRole))
        {
            foreach(var i in users)
            {
                if ((await _userManager.IsInRoleAsync(i, filters.UserRole)))
                    //users = users.Where(x=>x.Id !=  i.Id);
                    list.Add(await _userService.GetUserByIdAsync(i.Id));
            }

        }
        else
        {
            list = await users.ToListAsync();
        }
        //HttpContext.Session.SetString("page", "index");
        return PartialView("_UsersTableList", list);
    }

    [HttpGet]
    public async Task<IActionResult> UsersList()
    {
        var users = await _userService.GetAllAsync();
        //HttpContext.Session.SetString("page", "index");
        return PartialView("_UsersList", await users.ToListAsync());
    }

    #endregion



    [HttpGet]
    public async Task<IActionResult> Create(Guid id)
    {
        var user = await _userService.GetUserCreateDTOByIdAsync(id);
        if (user == null) return View();
        return View(user);

    }
    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDTO userCreateModel)
    {
        ApplicationUser user;
        try
        {
            //if (!ModelState.IsValid) { ModelState.AddModelError("", ModelState.Values.ToString()); return View(); }

            user = await _userService.GetUserByIdAsync(userCreateModel.Id);
            if(user!=null)
            _context.Entry(user).Reload();

            if (user != null)
            {
                try
                {
                    //var oo = SkillsHub.Application.Helpers.HashProvider.VerifyHash(userCreateModel.Password, user.PasswordHash);
                    //if (!oo) throw new Exception("Password not equal");
                    if(userCreateModel.Password != user.Password) throw new Exception("Password not equal");
                    //user = _mapper.Map<ApplicationUser>(userCreateModel);
                    user.FirstName = userCreateModel.FirstName;
                    user.LastName = userCreateModel.LastName;
                    user.Email = userCreateModel.Email;
                    user.Phone = userCreateModel.Phone;
                    user.Login = userCreateModel.Login;
                    user.BirthDate = userCreateModel.BirthDate;
                    user.Sex = userCreateModel.Sex;



                    _context.ApplicationUsers.Update(user);


                    await _context.SaveChangesAsync();
                }
                catch(Exception ex) { ModelState.AddModelError("", ex.Message); return View(); }
                

            }else
            {
                user = await _userService.CreateUserAsync(userCreateModel);

                //return RedirectToAction("Item", new { itemId = user.Id });
                if (userCreateModel.IsStudent)
                {
                    if (userCreateModel.IsTeacher) HttpContext.Session.SetString("isTeacher", "true");
                    return RedirectToAction("Create", "Student", new { id = user.Id });
                }
                if (userCreateModel.IsTeacher) return RedirectToAction("Create", "Teachers", new { id = user.Id });

            }
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(); }

        return RedirectToAction("Item", new { itemId = user.Id });
        //return View();
    }

    public async Task<IActionResult> Restore(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        user = await _userService.Restore(user);

        var returnUrl = HttpContext.Session.GetString("page");

        switch (returnUrl)
        {
            case "index": return RedirectToAction("Index");
            case "item": return RedirectToAction("Item", new { id = user.Id });
            default: return RedirectToAction("Index", "CRM");
        }
    }

    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        await _userService.SoftDeleteAsync(user);
        var returnUrl = HttpContext.Session.GetString("page");

        switch (returnUrl)
        {
            case "index": return RedirectToAction("Index");
            case "item": return RedirectToAction("Item",new {id = user.Id});
            default:return RedirectToAction("Index","CRM");
        }

    }

    public async Task<IActionResult> HardDelete(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        await _userService.HardDeleteAsync(user);
        var returnUrl = HttpContext.Session.GetString("page");

        switch (returnUrl)
        {
            case "index": return RedirectToAction("Index");
            case "item": return RedirectToAction("Item", new { id = user.Id });
            default: return RedirectToAction("Index", "CRM");
        }

    }

    [HttpGet]
    [Route("/Account/GetNotifications")]
    public async Task<IActionResult> GetNotifications()
    {
        var notifications = await _userService.GetCurrentUserNotifications();
        return PartialView("_Chat", notifications.OrderByDescending(x=>x.DateCreated).ToList());

        //return Json(JsonSerializerToAjax.GetJsonByIQueriable(notifications));
    }




    [HttpPost]
    public async Task<IActionResult> GetStudentGroupsByUser(Guid id, GroupFilterModel filters, OrderModel order)
    {
        var gr = _groupService.GetAll();
        List<Group> res = new List<Group>();


        var user = await _userService.GetUserByIdAsync(id);
        if (user.UserStudent == null) return PartialView("_StudentGroups", (user, res));
       
        var studentGroups = _groupService.GetAll().SelectMany(x=>x.GroupStudents)
            .Where(x=>x.Student.ApplicationUser.Id == user.Id).Select(x=>x.Group).ToList();
        

        var ress = await FilterMaster.FilterGroups(studentGroups.AsQueryable(), filters, order);
        var rerer = ress.ToList();

        foreach (var group in rerer)
            res.Add(await _groupService.GetAsync(group.Id));


        var otherLessons = _lessonService.GetAll()
            .Where(x => x.ArrivedStudents.Select(x => x.Id).Contains(user.UserStudent.Id))
            .Where(x => !studentGroups.Select(x => x.Id).Contains(x.Group.Id));




        return PartialView("_StudentGroups", (user, res, otherLessons.ToList()));

    }

    [HttpPost]
    public async Task<IActionResult> GetTeacherGroupsByUser(Guid id, GroupFilterModel filters, OrderModel order)
    {
        var gr = _groupService.GetAll();
        var user = await _userService.GetUserByIdAsync(id);
        List<Group> res = new List<Group>();

        if (user.UserTeacher == null) return PartialView("_TeacherGroups", (user, res));

        var teacherGroups = _groupService.GetAll().Include(x => x.Lessons).ThenInclude(x => x.ArrivedStudents).Where(x => x.GroupTeachers.Select(x=>x.Teacher).Select(x=>x.ApplicationUserId).Contains(user.Id)).ToList();


        //foreach (var group in teacherGroups)
        //  res.Add(await _groupService.GetAsync(group.Id));

        var ress = await FilterMaster.FilterGroups(teacherGroups.AsQueryable(), filters, order);
        var result = ress.ToList();

        var otherLessons = _lessonService.GetAll()
           .Where(x => x.Teacher.Id == user.UserTeacher.Id)
           .Where(x => !teacherGroups.Select(x => x.Id).Contains(x.Group.Id)).ToList();


        return PartialView("_TeacherGroups", (user, ress.ToList(), otherLessons));

    }



    [HttpGet]
    [AllowAnonymous]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(UserLoginDTO? user)
    {
        try
        {
            if (!ModelState.IsValid) { ModelState.AddModelError("", ModelState.Values.ToString()); return View(); }
            await _userService.InitialCreateAsync();
            var userDb = await _userService.SignInAsync(user);
            if (userDb == null) return View();
            return RedirectToAction("Index", "CRM");
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View(); }

    }
    [HttpGet]
    [AllowAnonymous]
    public async new Task<IActionResult> SignOut()
    {
        await _userService.SignOutAsync();
        
        return RedirectToAction("Index", "Home");

    }


}
