using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using NuGet.Protocol;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.Models;
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
    [Route("/Lesson/FilterLessons")]
    public async Task<IActionResult> FilterLessons(LessonFilterModel filters, OrderModel order)
    {
        var items =  _lessonService.GetAll();
        //items = items.Where(x => x.Group.Id == id).Where(x => x.IsDeleted == false);
        items = await FilterMaster.GetAllLessons(items, filters, order);

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


        //try
        //{
        //    var ii = items.ToList();
        //    var jsonString = HttpContext.Request.QueryString.Value ?? "";
        //    jsonString = jsonString.Substring(1); // Remove '?'
        //    jsonString = HttpUtility.UrlDecode(jsonString);

        //    var jsonObject = JObject.Parse(jsonString);
        //    var filterModel = jsonObject["LessonFilterModel"].ToObject<LessonFilterModel>();
        //    //var orderModel = jsonObject["LessonOrderModel"].ToObject<LessonOrderModel>();

           

        //}
        //catch (Exception ex) { }
        ////var viewName = Server.MapPath("~/Views/Group/_LessonsList.cshtml");
        //var res = System.IO.File.Exists("_LessonsList");

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

            //items = await FilterMaster.GetAllLessons(items, filterModel, orderModel);

        }
        catch (Exception ex) { }

        return Json(JsonSerializerToAjax.GetJsonByIQueriable(items));
    }

    #region Create
    [HttpGet]
    public async Task<IActionResult> Create(Guid id)
    {
        var group = await _groupService.GetAsync(id) ?? throw new Exception("Group not found");
        var duration = group.LessonType.LessonTimeInMinutes;

        ViewBag.grId = group.Id;
        ViewBag.DefaultLessonTime = group.LessonType.LessonTimeInMinutes;

        var item = new Lesson() { Group = group, GroupId = group.Id, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(duration) };
        return View("Item", item);
    }


    [HttpGet]
    public async Task<IActionResult> CreateBySchedule(Guid id) //groupId
    {
        var group = await _groupService.GetAsync(id);
        if (group == null) return RedirectToAction("Index", "Group");

        if (!group.IsUnlimitedLessonsCount && group.Lessons != null)
        {
            if (group.LessonsCount <= group.Lessons.Count())
                return PartialView("_MyError", "So much lessons");

            if (group.NeededLessonsTimeInMinutes <= group.ResultLessonsTimeInMinutes)
                return PartialView("_MyError", "So much working hours. Send message to administator to fix this problem");
        }


        //var item = new Lesson() { GroupId = group.Id };

        var dateStart = group.DateStart;
        var lessonsCount = group.LessonsCount;
        

        if (group.Lessons != null && group.Lessons.Count() > 0)
        {
            
            dateStart = group.Lessons.OrderByDescending(x => x.EndTime).FirstOrDefault().EndTime.AddDays(1); //Where(x=>x.EndTime <  DateTime.Now)
            lessonsCount = group.LessonsCount - group.Lessons.Count();//.Where(x => x.EndTime < DateTime.Now).Count();
        }

        bool isVerified = true;
        if (User.IsInRole("Teacher")) isVerified = false;

        await _groupService.CreateLessonsBySchedule(group.DaySchedules, dateStart, 1, group , isVerified);



        return RedirectToAction("Item", "Group", new {id = group.Id});
    }


    [HttpPost]
    public async Task<IActionResult> Create(Lesson lesson)
    {
        if (lesson.GroupId.HasValue)
        {
            var group = await _groupService.GetAsync(lesson.GroupId.Value) ?? throw new Exception("Group not found");
            var duration = group.LessonType.LessonTimeInMinutes;

            ViewBag.grId = group.Id;
            ViewBag.GroupId = _context.Groups.FirstOrDefaultAsync(x => x.Lessons.Select(x => x.Id).Contains(lesson.GroupId.Value));

        }
        
        //if(_context.Lessons.FirstOrDefaultAsync(x=>x.Id == lesson.Id) != null) return RedirectToAction("Edit", new {item = lesson});
        try
        {
            await _lessonService.Create(lesson);
        }
        catch(Exception ex) { ModelState.AddModelError("", ex.Message); lesson.Id = Guid.Empty; return View("Item", lesson); }
        return RedirectToAction("Item", "Group", new { id = lesson.GroupId });
    }
    #endregion



    //--------------------------------------------------------------------

    ///OK

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var lesson = _context.Lessons.Include(x => x.Group).AsNoTracking().FirstOrDefault(x => x.Id == id) ?? throw new Exception("Lesson not found");
        var lastLessonValue = await _context.Lessons.FirstOrDefaultAsync(x => x.Id == lesson.Id);

        await _notificationService.СreateToEditLesson(lastLessonValue, lesson, null, 1);
        await _context.SaveChangesAsync();

        var group = lesson.Group;//await _groupService.GetAll().FirstOrDefaultAsync(x => x.Lessons.Select(x => x.Id).Contains(id));
        await _requestService.DeletePreviousRequests(lesson);
        await _lessonService.DeleteLessonByGroup(group, lesson);

        return RedirectToAction("Item","Group",new { id = lesson.GroupId });
    }

    
    [HttpPost]
    public async Task<IActionResult> Edit(Lesson item)
    {

        if (item.GroupId.HasValue)
        {
            var group = await _groupService.GetAsync(item.GroupId.Value) ?? throw new Exception("Group not found");
            var duration = group.LessonType.LessonTimeInMinutes;

            ViewBag.grId = group.Id;
            ViewBag.GroupId = _context.Groups.FirstOrDefaultAsync(x => x.Lessons.Select(x => x.Id).Contains(item.GroupId.Value));

        }
        try
        {
            if (item.StartTime == DateTime.MinValue || item.StartTime.Year < DateTime.Now.Year - 10) throw new Exception("Not correct date");
            if (item.EndTime == DateTime.MinValue || item.EndTime.Year < DateTime.Now.Year - 10) throw new Exception("Not correct date");
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View("Item", item); }


        var lastLessonValue = await _context.Lessons.Include(x=>x.Group).FirstOrDefaultAsync(x=>x.Id == item.Id);
        var gr = new Group() { Id = lastLessonValue.Group.Id };
        _context.Entry(lastLessonValue.Group).State = EntityState.Detached;


        if(lastLessonValue != null &&lastLessonValue.StartTime != item.StartTime && lastLessonValue.EndTime != item.EndTime)
        {
            await _notificationService.СreateToEditLesson(lastLessonValue, item, null, 1);
            await _context.SaveChangesAsync();
        }


        item.Group = gr;
        _context.Entry(item.Group).State = EntityState.Unchanged;
        _context.Lessons.Update(item);
        await _context.SaveChangesAsync();

        //await _lessonService.UpdateStudentsByLesson(item, studentId.ToList());


        return RedirectToAction("Item", "Group", new { id = lastLessonValue.Group.Id });
    }

    [HttpPost]
    public async Task SaveStudentsByLesson(Guid[] visitStudent, Guid[] passedStudent, Guid lessonId)
    {
        foreach(var i in visitStudent)
        {
            var ii = await _context.LessonStudents.Include(x=>x.Lesson).FirstOrDefaultAsync(x => x.Id == i && x.Lesson.Id == lessonId);
            ii.IsVisit = true;
            _context.LessonStudents.Update(ii);
        }
        foreach(var i in passedStudent)
        {
            var ii = await _context.LessonStudents.Include(x => x.Lesson).FirstOrDefaultAsync(x => x.Id == i && x.Lesson.Id == lessonId);
            ii.IsVisit = false;
            _context.LessonStudents.Update(ii);
        }
        await _context.SaveChangesAsync();

    }

    [HttpGet]
    public async Task<IActionResult> Item(Guid id)
    {
        var lesson = await  _lessonService.GetAsync(id);
      
        //ViewBag.studentsByLesson = lesson.ArrivedStudents;
        
        ViewBag.GroupId =_context.Groups.FirstOrDefaultAsync(x=>x.Lessons.Select(x=>x.Id).Contains(id));
        
        

        //if (lesson == null) return Redirect("Create",new Lesson() { GroupId = });
        return View(lesson);

    }

    [HttpPost]
    public async Task<IActionResult> GetArrivedStudentsByLesson(Guid id)
    {
        var lesson = await _context.Lessons.Include(x=>x.Group).ThenInclude(x=>x.GroupStudents).ThenInclude(x=>x.Student).ThenInclude(x=>x.ApplicationUser).Include(x=>x.ArrivedStudents).ThenInclude(x=>x.Student).ThenInclude(x=>x.ApplicationUser).FirstOrDefaultAsync(x=>x.Id == id);

        var groupSt = new List<Student>();//;

        if (lesson == null) return PartialView("_ArrivedStudentByLesson");
        var lessonStudents = lesson.ArrivedStudents;
        var stst = lessonStudents.Select(x => x.Student).ToList(); // Convert to List

        var students = _context.Students.Include(x=>x.ApplicationUser);
        //students.RemoveRange(stst);

        foreach (var i in stst)
        {
            _context.Entry(i).State = EntityState.Detached;
        }

        var ggg = lesson.Group.GroupStudents.Select(x => x.Student).ToList();
        foreach (var student in ggg)
        {
            if (!stst.Select(x => x.Id).Contains(student.Id))
                groupSt.Add(student);
        }




        ViewBag.LessonId = id;
        return PartialView("_ArrivedStudentByLesson", (lesson.ArrivedStudents, students.ToList(), groupSt));

    }

    [HttpPost]
    public async Task<ActionResult> CreateArrivedStudent(Guid lessonId, Guid userId)
    {
        var student = await _context.Students.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.ApplicationUser.Id == userId);
        var lesson = await _lessonService.GetAsync(lessonId);

        if (await _context.LessonStudents.FirstOrDefaultAsync(x => x.Lesson.Id == lessonId && x.Student.Id == student.Id) != null) return Json(null);

        var lessStudent = new LessonStudent() { Student = student, Lesson = new Lesson() { Id = lesson.Id } };
        _context.Entry(lessStudent.Student).State = EntityState.Unchanged;
        _context.Entry(lessStudent.Lesson).State = EntityState.Unchanged;
        await _context.LessonStudents.AddAsync(lessStudent);
        await _context.SaveChangesAsync();
        var result = new
        {
            message = "Successfully removed arrived student"
        };
        return Json(result);
    }

    [HttpPost]
    public async Task<ActionResult> RemoveArrivedStudent(Guid lessonId, Guid userId)
    {
        var users = await _userService.GetAllAsync();
        var student = await users.FirstOrDefaultAsync(x => x.Id == userId);

        var lessStudent = await _context.LessonStudents.FirstOrDefaultAsync(x => x.Lesson.Id == lessonId && x.Student.Id == student.UserStudent.Id);
        if (lessStudent == null) return Json(null);
        _context.LessonStudents.Remove(lessStudent);
        await _context.SaveChangesAsync();
        var result = new
        {
            message = "Successfully removed arrived student"
        };
        return Json(result);
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
