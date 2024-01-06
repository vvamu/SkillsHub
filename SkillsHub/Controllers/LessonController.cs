using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Helpers;
using SkillsHub.Helpers.SearchModels;
using SkillsHub.Persistence;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Web;

namespace SkillsHub.Controllers;

[Authorize]
public class LessonController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;
    private readonly IRequestService _requestService;
    private readonly INotificationService _notificationService;
    private readonly ILessonService _lessonService;
    private readonly IGroupService _groupService;

    public LessonController(ApplicationDbContext context, IUserService userService, IRequestService requestService, 
        INotificationService notificationService,ILessonService lessonService, IGroupService groupService)
    {
        _context = context;
        _userService = userService;
        _requestService = requestService;
        _notificationService = notificationService;
        _lessonService = lessonService;
        _groupService = groupService;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var lessons = await _context.Lessons.ToListAsync();
        return View(lessons);
    }

    [HttpGet]
    [Route("/Lessons/GetAllByGroup")]

    public async Task<IActionResult> GetAllByGroupJSON(Guid id)
    {
        var items = _context.Lessons.Where(x => x.IsDeleted == false).Where(x => x.Group.Id == id).OrderBy(x=>x.StartTime);

        return Json(JsonSerializerToAjax.GetJsonByIQueriable(items));

        //return View();
    }

    [HttpPost]
    [Route("/Lesson/GetByGroup")]
    public async Task<IActionResult> GetByGroup(Guid id, StudentFilterModel filters, StudentOrderModel order)
    {
        var items =  _lessonService.GetAll().ToList();
        items = items.Where(x => x.Group.Id == id).Where(x => x.IsDeleted == false).ToList();
        /*
        try
        {
            var group = await _context.Groups.Include(x=>x.Lessons).ThenInclude(x=>x.Teacher)
                .Include(x=>x.Teacher)
                .FirstOrDefaultAsync(x=>x.Id == id);

            foreach(var item in items)
            {
                item.Group = group;
                if (item.Teacher != null)
                {
                    item.Teacher = group.Teacher;
                    
                }
                

                if (!group.Lessons.Select(x=>x.Id).Contains(item.Id))
                {
                    if (group.Lessons != null)
                        group.Lessons.Add(item);
                    group.Lessons = new List<Lesson>() { item };

                }
                
                _context.Entry(item.Group).State = EntityState.Unchanged;
                //_context.Entry(group.Lessons).State = EntityState.Unchanged;
                //_context.Entry(item.Teacher).State = EntityState.Unchanged;
                _context.Lessons.Update(item);
                //_context.Groups.Update(group);
            }
           
            await _context.SaveChangesAsync();
        }
        catch (Exception ex){ }
    */


        try
        {
            var ii = items.ToList();
            var jsonString = HttpContext.Request.QueryString.Value ?? "";
            jsonString = jsonString.Substring(1); // Remove '?'
            jsonString = HttpUtility.UrlDecode(jsonString);

            var jsonObject = JObject.Parse(jsonString);
            var filterModel = jsonObject["LessonFilterModel"].ToObject<LessonFilterModel>();
            //var orderModel = jsonObject["LessonOrderModel"].ToObject<LessonOrderModel>();

            //items = await FilterMaster.GetAllLessons(items, filterModel, null);

        }
        catch (Exception ex) { }
        //var viewName = Server.MapPath("~/Views/Group/_LessonsList.cshtml");
        var res = System.IO.File.Exists("_LessonsList");

        return PartialView("_LessonsList", items.ToList());

        //return View();
    }


    [HttpGet]
    [Route("/Lessons/GetAllAsync")]
    public async Task<IActionResult> GetAllAsync()
    {
        var items = _context.Lessons.Include(x=>x.Group).ThenInclude(x=>x.Teacher)
            .Include(x=>x.Group).ThenInclude(x=>x.GroupStudents)
            .Include(x=>x.LessonTask)
            .Include(x=>x.LessonType).AsQueryable();

        try
        {
            var jsonString = HttpContext.Request.QueryString.Value ?? "";
            jsonString = jsonString.Substring(1); // Remove '?'
            jsonString = HttpUtility.UrlDecode(jsonString);

            var jsonObject = JObject.Parse(jsonString);
            var filterModel = jsonObject["LessonFilterModel"].ToObject<LessonFilterModel>();
            var orderModel = jsonObject["LessonOrderModel"].ToObject<LessonOrderModel>();

            items = await FilterMaster.GetAllLessons(items, filterModel, orderModel);

        }
        catch (Exception ex) { }

        return Json(JsonSerializerToAjax.GetJsonByIQueriable(items));
    }

    #region Create
    [HttpGet]
    public async Task<IActionResult> Create(Guid itemId)
    {
        var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == itemId) ?? throw new Exception("Group not found");
        var item = new Lesson() { GroupId = group.Id };
        return View(item);
    }


    [HttpGet]
    public async Task<IActionResult> CreateBySchedule(Guid id) //groupId
    {
        var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Group not found");


        var item = new Lesson() { GroupId = group.Id };
        return View(item);
    }


    [HttpPost]
    public async Task<IActionResult> Create(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
        await _context.SaveChangesAsync();
        return View();
    }
    #endregion



    //--------------------------------------------------------------------

    ///OK
    [HttpPost]
    public async Task<IActionResult> Edit(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
        return RedirectToAction("Item", "Group", new { id = lesson.GroupId });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var lesson = _context.Lessons.Include(x=>x.Group).AsNoTracking().FirstOrDefault(x => x.Id == id) ?? throw new Exception("Lesson not found");
        var group = lesson.Group;//await _groupService.GetAll().FirstOrDefaultAsync(x => x.Lessons.Select(x => x.Id).Contains(id));
        //lesson.GroupId = group.Id;

        try
        {

        
        var message = "Lesson was deleted by administator";
        if(lesson.StartTime < DateTime.Now)
        {

            //await _requestService.Create(lesson.Id, message);
            await _notificationService.CreateToLesson(lesson, null, null, 1);

                try
                {
                    await _context.RequestLessons.Include(x => x.LessonBefore).Where(x => x.LessonBefore.Id == id).ForEachAsync(x => _context.RequestLessons.Remove(x));
                    await _context.SaveChangesAsync();
                }catch { }
                try
                {
                    group.Lessons.Remove(lesson);

                    _context.Groups.Update(group);
                    _context.Lessons.Remove(lesson);
                }
                catch { }
            //group = lesson.Group;
            

        }
        else
        {
            lesson.IsDeleted = true;
            _context.Lessons.Update(lesson);

        }
        await _context.SaveChangesAsync();
        }
        catch (Exception ex) { }
        return RedirectToAction("Item","Group",new { id = lesson.GroupId });
    }


    [HttpGet]
    public async Task<IActionResult> Item(Guid id)
    {
        var lesson =await  _lessonService.GetAsync(id);
        ViewBag.studentsByLesson = lesson.ArrivedStudents;
        ViewBag.GroupId =_context.Groups.FirstOrDefaultAsync(x=>x.Lessons.Select(x=>x.Id).Contains(id));
        //ViewBag.teacher

        if (lesson == null) return View("Create");
        return View(lesson);

    }


    [HttpPost]
    public async Task<IActionResult> RemoveUserFromLesson(Guid id,string message)
    {
        var lesson = await _lessonService.GetAsync(id);
        var user = await _userService.GetCurrentUserAsync();
        var lessonStudent = lesson.ArrivedStudents.FirstOrDefault(x => x.StudentId == user.UserStudent.Id);
        lessonStudent.IsVisit = false;
        _context.LessonStudents.Update(lessonStudent);
        return RedirectToAction("Item", new{ id = id });

    }




}
