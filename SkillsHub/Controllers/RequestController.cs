//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SkillsHub.Application.Services.Implementation.User;
//using SkillsHub.Persistence;

//namespace SkillsHub.Controllers;

//[Authorize]
//public class RequestController : Controller
//{
//    private readonly ApplicationDbContext _context;
//    private readonly IUserService _userService;

//    public RequestController(ApplicationDbContext context, IUserService userService, )
//    {
//        _context = context;
//        _userService = userService;
//        _requestService = requestService;
//        //_lessonService = lessonService;
//    }
//    [HttpGet]
//    public async Task<IActionResult> Index()
//    {
//        var items = await _requestService.GetAll();
//        items.Where(x => x.IsDeleted == false);
//        return View(items.AsQueryable());
//    }

//    #region CreateReq


//    [HttpPost]
//    public async Task<IActionResult> RequestEdit(Lesson item)
//    {
//        var user = await _userService.GetCurrentUserAsync();

//        var requestMessage = user.Login.ToString()
//            + " want to update current lesson with values: "
//            + "\nStart time : " + item.StartTime.ToShortDateString() + " " + item.StartTime.ToShortTimeString()
//            + "\nEnd time : " + item.EndTime.ToShortDateString() + " " + item.EndTime.ToShortTimeString();

//        var request = await _requestService.Create(item.Id, requestMessage, item);

//        return RedirectToAction("Item", "Group", new { id = request.LessonBefore.GroupId });
//    }
//    [HttpPost]
//    public async Task<IActionResult> RequestDelete(Guid id)
//    {
//        var user = await _userService.GetCurrentUserAsync();
//        var requestMessage = user.Login.ToString() + " want to delete current lesson";
//        var req = await _requestService.Create(id, requestMessage, null);
//        //await _requestService.ApplyLessonRequest(req, null, 1);

//        return RedirectToAction("Item", "Group", new { id = req.LessonBefore.GroupId });
//    }

//    #endregion

//    public async Task<IActionResult> ApplyRequest(Guid id, RequestLesson item) //in button value apply or no apply
//    {

//        var req = await _context.RequestLessons.Include(x => x.LessonBefore).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
//        req.NewStart = item.NewStart;
//        req.NewEnd = item.NewEnd;
//        await _requestService.ApplyLessonRequest(req, 1);
//        //var gr = await _context.Lessons.FirstOrDefaultAsync(x => x.Id == id);

//        return RedirectToAction("Index", "Request");
//    }
//    public async Task<IActionResult> ApplyDeleteRequest(Guid id, Lesson lesson) //in button value apply or no apply
//    {

//        var req = await _context.RequestLessons.Include(x => x.LessonBefore).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
//        await _requestService.ApplyLessonDeleteRequest(req, 1);
//        //var gr = await _context.Lessons.FirstOrDefaultAsync(x => x.Id == id);

//        return RedirectToAction("Index", "Request");
//    }
//    public async Task<IActionResult> RejectRequest(Guid id, Lesson lesson) //in button value apply or no apply
//    {

//        var req = await _context.RequestLessons.Include(x => x.LessonBefore).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
//        await _requestService.ApplyLessonRequest(req, -1);

//        return RedirectToAction("Index", "Request");
//    }
//    public async Task<IActionResult> RejectDeleteRequest(Guid id, Lesson lesson) //in button value apply or no apply
//    {

//        var req = await _context.RequestLessons.Include(x => x.LessonBefore).ThenInclude(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
//        await _requestService.ApplyLessonDeleteRequest(req, -1);

//        return RedirectToAction("Index", "Request");
//    }
//    #region Service




//    #endregion
//}
