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

       
        return PartialView("_LessonsList", items.ToList());

        //return View();
    }


    [HttpGet]
    [Route("/Lessons/GetAllAsync")]
    public async Task<IActionResult> GetAllAsync()
    {
        var items = _context.Lessons.Include(x=>x.Group).ThenInclude(x=>x.GroupTeachers)
            .Include(x=>x.Group).ThenInclude(x=>x.GroupStudents)
            .Include(x=>x.LessonTask)
            .AsQueryable();

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
            /*
            if (group.NeededLessonsTimeInMinutes <= group.ResultLessonsTimeInMinutes)
                return PartialView("_MyError", "So much working hours. Send message to administator to fix this problem");
            */
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

        var lessons = await _groupService.CreateLessonsBySchedule(group.DaySchedules, dateStart, 1, group , isVerified);
        await _notificationService.СreateToUpdateCountLessonsInGroup(group,lessonsCount,lessonsCount+1,null);



        return RedirectToAction("Item", "Group", new {id = group.Id});
    }


    [HttpPost]
    public async Task<IActionResult> Create(Lesson lesson)
    {
        lesson.Id = Guid.Empty;
        Guid? grId = lesson.GroupId.HasValue ? lesson.GroupId.Value : lesson.Group?.Id;
        Group group = new Group();
        int previousCountLessons = 0;
        if (grId != null)
        {
            group = await _groupService.GetAsync(grId.Value) ?? throw new Exception("Group not found");
            previousCountLessons = group.Lessons.Count();
            var duration = group.LessonType.LessonTimeInMinutes;

            ViewBag.grId = group.Id;
            ViewBag.GroupId = group.Id;

        }
        try
        {
            await _lessonService.Create(lesson);
        }
        catch(Exception ex) { ModelState.AddModelError("", ex.Message); lesson.Id = Guid.Empty; return View("Item", lesson); }
        #region Notification
            await _notificationService.СreateToUpdateCountLessonsInGroup(group, previousCountLessons, previousCountLessons + 1, null);
        #endregion


        return RedirectToAction("Item", "Group", new { id = lesson.GroupId });
    }
    #endregion



    //--------------------------------------------------------------------

    ///OK

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var lesson = _context.Lessons.Include(x => x.Group).AsNoTracking().FirstOrDefault(x => x.Id == id) ?? throw new Exception("Lesson not found");

        #region Notification
        await _notificationService.СreateToEditLesson(lesson, null, null, 1);
        await _context.SaveChangesAsync();
        #endregion

        var group = lesson.Group;

        #region Request
        await _requestService.DeletePreviousRequests(lesson);
        #endregion
        
        await _lessonService.DeleteLessonByGroup(group, lesson);

        return RedirectToAction("Item","Group",new { id = lesson.GroupId });
    }

    
    [HttpPost]
    public async Task<IActionResult> Edit(Lesson item)
    {
        int duration = 40;
        if (item.GroupId.HasValue)
        {
            var group = await _groupService.GetAsync(item.GroupId.Value) ?? throw new Exception("Group not found");
            duration = group.LessonType.LessonTimeInMinutes;

            ViewBag.grId = group.Id;
            ViewBag.GroupId = await _context.Groups.FirstOrDefaultAsync(x => x.Lessons.Select(x => x.Id).Contains(item.GroupId.Value));

        }
        
        try
        {
            var  ressss= await _lessonService.Edit(item);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View("Item", item); }

       


        #region CreateNotification

        var lastLessonValue = await _context.Lessons.Include(x=>x.Group).FirstOrDefaultAsync(x=>x.Id == item.Id);
        var gr = new Group() { Id = lastLessonValue.Group.Id };
        _context.Entry(lastLessonValue.Group).State = EntityState.Detached;

        bool editedTime = lastLessonValue != null && lastLessonValue.StartTime != item.StartTime && lastLessonValue.EndTime != item.EndTime;
        
        item.Group = gr;
        _context.Entry(item.Group).State = EntityState.Unchanged;
        //_context.Lessons.Update(item);
        //await _context.SaveChangesAsync();


        if (editedTime)
        {
            await _notificationService.СreateToEditLesson(lastLessonValue, item, null, 1);
            await _context.SaveChangesAsync();
            //return RedirectToAction("Edit", "Request", new {item = lastLessonValue });
        }
        #endregion

        //await _lessonService.UpdateLessonStudents(item, studentId.ToList());


        return RedirectToAction("Item", "Group", new { id = lastLessonValue.Group.Id });
    }

    [HttpPost]
    public async Task ChangeArrivedStudentStatus(Guid id,  int neededStatus)
    {
        var item = await _context.LessonStudents.FirstOrDefaultAsync(x => x.Id == id);
        if (item == null) return;
        item.VisitStatus = neededStatus;
        _context.LessonStudents.Update(item);
        await _context.SaveChangesAsync();

    }
    /*
    [HttpPost]
    public async Task SaveStudentsByLesson(Guid[] visitStudent, Guid[] passedStudent, Guid lessonId)
    {
        foreach(var i in visitStudent)
        {
            var ii = await _context.LessonStudents.Include(x=>x.Lesson).FirstOrDefaultAsync(x => x.Id == i && x.Lesson.Id == lessonId);
            ii.VisitStatus = true;
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
    */

    [HttpGet]
    public async Task<IActionResult> Item(Guid id)
    {
        var lesson = await  _lessonService.GetAsync(id);
        ViewBag.GroupId =_context.Groups.FirstOrDefaultAsync(x=>x.Lessons.Select(x=>x.Id).Contains(id));
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

        if (lesson.Group.IsPermanentStaffGroup)
        {
            var ccc = await _userService.GetAllStudentsAsync();
            ccc = ccc.Where(x => x.IsDeleted == false);
            ggg = ccc.ToList();
        }

        foreach (var student in ggg)
        {
            if (!stst.Select(x => x.Id).Contains(student.Id))
                groupSt.Add(student);
        }
        




        ViewBag.LessonId = id;
        return PartialView("_ArrivedStudentByLesson", (lesson.ArrivedStudents, students.ToList(), groupSt, lesson.Group.IsPermanentStaffGroup));

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


    //TODO

    [HttpPost]
    public async Task<IActionResult> RequestFromStudentToMissLesson(Guid id,string message)
    {
        var lesson = await _lessonService.GetAsync(id);
        var user = await _userService.GetCurrentUserAsync();
        var lessonStudent = lesson.ArrivedStudents.FirstOrDefault(x => x.StudentId == user.UserStudent.Id);
        //lessonStudent.IsVisit = false;
        _context.LessonStudents.Update(lessonStudent);
        return RedirectToAction("Item", new{ id = id });

    }




}
