using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Persistence;

namespace SkillsHub.Controllers;

public class GroupController : Controller
{
    private readonly IGroupService _groupService;

    private readonly ICourcesService _courcesService;
    private readonly ApplicationDbContext _context;

    public GroupController(IGroupService groupService,ICourcesService courcesService, ApplicationDbContext context)
    {
        _groupService = groupService;
        _courcesService = courcesService;
        _context = context;
    }
    public IActionResult Index()
    {
        ViewBag.CourceNames = _courcesService.GetAllCourcesNames();
        //ViewBag.Students = 
        return View(_groupService.GetAll());
    }
    public async Task<IActionResult> Item(Guid id)
    {
        var group = await _groupService.GetAsync(id);
        return View(group);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Group item, Guid[] itemValue, Guid[] teacherValue, string[] dayName, TimeSpan[] startTime, int[] duration)
    {
        try
        {

            //string selected = Request.Form["itemValue"];
            item.TeacherId = teacherValue[0];

            var group = await _groupService.CreateAsync(item);
            await _groupService.AddStudentsToGroupAsync(group.Id, itemValue.ToList());

            for (int i = 0; i < dayName.Count(); i++)
            {
                var scheduleDay = new UserDaySchedule()
                {
                    DayName = Enum.Parse<DayOfWeek>(dayName[i]),
                    WorkingStartTime = startTime[i],
                    WorkingEndTime = startTime[i] + TimeSpan.FromMinutes(duration[i]),
                    Group = group
                };
                _context.DaySchedules.Add(scheduleDay);
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message); return View();
        }
        return RedirectToAction("Index");
    }
    public IActionResult GetCreateModal()
    {
        return PartialView("_Create");
    }

}
