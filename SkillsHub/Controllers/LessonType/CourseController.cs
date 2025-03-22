using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Repository.Base;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Persistence;

namespace SkillsHub.Controllers;

public class CourseController : Controller
{
    private readonly IAbstractLogModelService<Course> _courseService;
    private readonly ApplicationDbContext _context;
    private readonly IUploadImageService<Course> _uploadImageService;
    private readonly IUploadIconService<Course> _uploadIconService;

    public CourseController(ApplicationDbContext context, 
        IAbstractLogModelService<Course> courseService, IUploadImageService<Course> uploadImageService,IUploadIconService<Course> uploadIconService)
    {
        _context = context;
        _courseService = courseService as CourseService;
        
        _uploadImageService = uploadImageService;
        _uploadIconService = uploadIconService;
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCourseByName(string? name, string? position, string? imagePath, bool isEditMode = false)
    {
        if (string.IsNullOrEmpty(name)) return PartialView("./Views/Home/Index/Courses/Course.cshtml", new Course());

        var parts = name.Split("-");
        var items = await _context.Courses.Where(x => x.ParentId == null).Where(x => x.IsVisibleOnMainPage).ToListAsync();
        foreach (var part in parts)
        {
            items = items.Where(x => x.IdentityString.Contains(part)).ToList(); //x.DisplayName.ToLower().Contains(part.ToLower())).ToList();
        }

        var item = items.FirstOrDefault();
        if (item == null) return null;
        if (!string.IsNullOrEmpty(position)) item.PositionOnSite = position;
        item.IsEditMode = isEditMode;
        item.IsVisibleOnMainPage = true;

        #region image and string identity
        var itemName = item.DisplayName.ToLower();
        var newPathToImage = item.PathToImage;
        var newImageIdentity = item.IdentityString;


        //if (itemName.Contains("взросл"))
        //{
        //    newPathToImage = "/images/course/courses_adult_english@0x.jpg";
        //    newImageIdentity = "english-adults";
        //    item.OrderOnMainPage = 1;
        //    item.DescriptionOnMainPageMore = "Для взрослых занятия ведутся с упором на разговорную речь, уделяется внимание заполнению пробелов в грамматике.";
        //    item.PathToIcon = "/images/icon_course/admission.png";
        //}
        //if (itemName.Contains("англ") && itemName.Contains("дет"))
        //{
        //    newPathToImage = "/images/course/courses_children@0x.jpg";
        //    newPathToImage = "/images/fiol2.jpg";
        //    newImageIdentity = "english-children";
        //    item.OrderOnMainPage = 2;
        //    item.DescriptionOnMainPageMore = "Для детей изучение английского языка проходят в игровой форме, что позволяет ребенку легко запоминать материал.";
        //    item.PathToIcon = "/images/icon_course/career.png";
        //}
        //if (itemName.Contains("организ"))
        //{
        //    newPathToImage = "/images/course/courses_organization_english@0x.jpg";
        //    newImageIdentity = "english-organization";
        //    item.OrderOnMainPage = 3;
        //    item.DescriptionOnMainPageMore = "Корпоративные курсы с учетом специфики бизнеса. Создаем возможности работать с иностранными компаниями.";
        //    item.PathToIcon = "/images/icon_course/visa.png";

        //}
        //if (itemName.Contains("англ") && itemName.Contains("экспресс"))
        //{
        //    newPathToImage = "/images/course/courses_express_english@0x.jpg";
        //    newImageIdentity = "english-express";
        //    item.OrderOnMainPage = 4;
        //    item.DescriptionOnMainPageMore = "Курсы с выбранной вами спецификой. Смотреть фильмы, играть, читать научную литературу - выбирать тебе.";
        //    item.PathToIcon = "/images/icon_course/clock.png";

        //}
        //if (itemName.Contains("англ") && itemName.Contains("разговорный"))
        //{
        //    newPathToImage = "/images/course/courses_speaking_club@0x.jpg";
        //    newImageIdentity = "english-speaking";
        //    item.OrderOnMainPage = 5;
        //    item.DescriptionOnMainPageMore = "Занятия с упором на свободное общение. Совершенствуйте свои разговорные навыки вместе с носителями языка.";
        //    item.PathToIcon = "/images/icon_course/departure.png";
        //}
        //if (itemName.Contains("программирование") && itemName.Contains("дет"))
        //{
        //    newPathToImage = "/images/course/courses_programming@0x.jpg";
        //    newImageIdentity = "programming-children";
        //    item.OrderOnMainPage = 6;
        //    item.DescriptionOnMainPageMore = "Освойте навыки программирования и проложите свой путь к успеху.";
        //    item.PathToIcon = "/images/icon_course/toy.png";

        //}

        //if (newPathToImage != item.PathToImage)
        //{
        //    item.PathToImage = newPathToImage;

        //}
        #endregion
        item.IdentityString = newImageIdentity;
        _context.Courses.Update(item); _context.SaveChanges();


        //if (string.IsNullOrEmpty(item?.DescriptionOnMainPageMore) && item != null) item.DescriptionOnMainPage = " Групповой формат (до 8 человек) \n- Онлайн/оффлайн \n- Занятия проводятся 2 раза в неделю по 70 минут\n- \n- Индивидуальный формат \n- Онлайн/оффлайн \n- Занятия проводятся по 60 минут \n--\n- Длительность зависит от скорости обучения учеников";
        return PartialView("./Views/Home/Index/Courses/Course.cshtml", item ?? new Course());
    }
    
    [HttpGet]

    public async Task<IActionResult> GetCoursesOverview(bool isEditMode = false)
    {
        var items = _context.Courses.Where(x => (x.ParentId == null || x.ParentId == Guid.Empty) && !string.IsNullOrEmpty(x.IdentityString)).Where(x => x.IsVisibleOnMainPage).OrderBy(x => x.OrderOnMainPage).ToList();
        items.ForEach(x => x.IsEditMode = isEditMode);
        //foreach(var i in items)
        //{
        //    string parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        //    var localPath = parentDirectory + "\\wwwroot\\images\\icon_course";
        //    string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //    if (environment == "Development")
        //    {
        //        localPath = parentDirectory + "\\SkillsHub\\wwwroot\\images\\icon_course";
        //    }
        //    var fileNameDb = Path.GetFileName(i.PathToIcon);
        //    fileNameDb = localPath + fileNameDb;
        //    var imageDbLenght = Path.Exists(fileNameDb) ? new System.IO.FileInfo(fileNameDb).Length : 0;
        //    if (imageDbLenght == 0) i.PathToIcon = "/images/icon_course/error.png";
        //}
        
        return PartialView("./Views/Home/Index/Courses/CoursesOverviewBlock.cshtml", items ?? null);
    }

    public async Task<IActionResult> Index()
    {
        var items = _courseService.GetItems(onlyCurrent: true);
        var result = await items.ToListAsync();
        return PartialView(result);
    }

    public async Task<string> ChangeVisibleStatusOnPage(string id)
    {
        Guid itemId;
        if (!Guid.TryParse(id, out itemId)) return "";
        var item = await _context.Courses.Where(x => x.ParentId == null).FirstOrDefaultAsync(x=>x.Id == itemId);
        item.IsVisibleOnMainPage = false;
        _context.Courses.Update(item);
        await _context.SaveChangesAsync();
        return item.IdentityString;

    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(Course item)
    {
        try
        {
            await _uploadImageService.UploadImage(item);
        }
        catch(Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        
        return Json("Compete successfully");
    }

    

        [HttpPost]
    public async Task<IActionResult> UploadIcon(Course item)
    {
        try
        {
            Console.Write("");
            await _uploadIconService.UploadIcon(item);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        return Json("Compete successfully");
    }


    [HttpGet]
    public async Task<IActionResult> Create(Guid? itemId)
    {
        var res = await _courseService.GetLastValueAsync(itemId);
        var firstSymbol = res.DescriptionOnMainPage?.Substring(0,1);
        if (firstSymbol == "\n") res.DescriptionOnMainPage = res?.DescriptionOnMainPage.Substring(1).Replace("\n\n","");
        return View(res);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Course item)
    {
        Course? result;
        try
        {
            var id = Guid.Empty;
            Guid.TryParse(item.StringId, out id);
            if (item.Id == Guid.Empty && id != Guid.Empty) item.Id = id;
            var isEdit = await _context.Courses.FindAsync(item.Id);
            if (isEdit != null) result = await _courseService.UpdateAsync(item);
            else result = await _courseService.CreateAsync(item);
        }
        catch (Exception ex) 
        { 
            ModelState.AddModelError("", ex.Message); 
            var id = Guid.Empty;
            Guid.TryParse(item.StringId, out id);
            item.Id = id;
            var res2 = await _courseService.GetLastValueAsync(item.Id, withParents: true); 
            return View(res2);
            
        }
        var res = await _courseService.GetLastValueAsync(result.Id);
        return View(res);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeActiveStatus(Course item, bool isRemove = false)
    {
        try
        {
            var id = Guid.Empty;
            Guid.TryParse(item.StringId, out id);
            if (item.Id == Guid.Empty && id != Guid.Empty) item.Id = id;
            if (isRemove) { item = await _courseService.RemoveAsync(item.Id); return RedirectToAction("InstitutionSetting", "Home"); }
            else item = await _courseService.RestoreAsync(item.Id);
        }
        catch (Exception ex) { ModelState.AddModelError("", ex.Message); return View("Create", item); }
        return RedirectToAction("Create", item);
    }
    /*
    [HttpPost]
    public async Task<IActionResult> OptionsList(string subjectId,string courseId)
    {
        var courses = _context.Courses.AsAsyncQueryable();
        var subjId = Guid.TryParse(subjectId?.ToString(), out var result) ? result : Guid.Empty;
        var coursId = Guid.TryParse(courseId?.ToString(), out var resultC) ? resultC : Guid.Empty;

       // if (subjId != Guid.Empty) courses = courses.Where(x => x.SubjectId == subjId);

        return PartialView((courses, coursId));    
    }*/

}