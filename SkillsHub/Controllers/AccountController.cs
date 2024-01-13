using AutoMapper;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
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

namespace SkillsHub.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly ICourcesService _courcesService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IGroupService _groupService;

    public AccountController(IUserService userService, ICourcesService courcesService,
        ApplicationDbContext context, IMapper mapper, IGroupService groupService)
    {
        _userService = userService;
        _courcesService = courcesService;
        _context = context;
        _mapper = mapper;
        _groupService = groupService;

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
        ViewBag.CourcesNames = _courcesService.GetAllCourcesNames();
        ViewBag.EnglishLevels = "";
        ViewBag.LessonTypes = _courcesService.GetAllLessonType();
        ApplicationUser? user;
        if (itemId == Guid.Empty)
            itemId = id;

        user = await _userService.GetUserByIdAsync(itemId);
        if (user == null)
            user = await _userService.GetCurrentUserAsync();
        HttpContext.Session.SetString("page", "item");

        return View(user);
    }

    [HttpPost]
    [Route("/Account/UsersTableList")]

    public async Task<IActionResult> UsersTableList(UserFilterModel filters, OrderModel order)
    {
        var users = await _userService.GetAllAsync();
        users = await FilterMaster.FilterUsers(users, filters,order);
        //HttpContext.Session.SetString("page", "index");
        var ku = users.ToList();
        return PartialView("_UsersTableList", await users.ToListAsync());
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
                    if (user.Password != userCreateModel.Password) throw new Exception("Password not equal");
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
                catch(Exception ex) { }
                

            }else
            {
                user = await _userService.CreateUserAsync(userCreateModel);
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
        return PartialView("_Chat", notifications.ToList());

        //return Json(JsonSerializerToAjax.GetJsonByIQueriable(notifications));
    }

    [HttpPost]
    public async Task<IActionResult> GetGroupsByUser(Guid id)
    {
        var gr = _groupService.GetAll();
        foreach(var g in gr)
        {

            await _groupService.HardDeleteAsync(g.Id);
        }
        var user = await _userService.GetUserByIdAsync(id);
        var studentGroups = _groupService.GetAll().Include(x=>x.Lessons).SelectMany(x=>x.GroupStudents).Where(x=>x.Student.ApplicationUser.Id == user.Id).Select(x=>x.Group).ToList();
        var teacherGroups = _groupService.GetAll().Include(x=>x.Lessons).ThenInclude(x=>x.ArrivedStudents).Where(x => x.Teacher.ApplicationUser.Id == id).ToList();

        return PartialView("_UserGroups", (user, studentGroups, teacherGroups));

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
