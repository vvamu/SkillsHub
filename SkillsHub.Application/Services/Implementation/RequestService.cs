using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

public class RequestService : IRequestService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;
    private readonly ILessonService _lessonService;

    public RequestService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, IUserService userService,
        INotificationService notificationService, ILessonService lessonService)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
        _userService = userService;
        _notificationService = notificationService;
        _lessonService = lessonService;
    }
    


    public async Task<RequestLesson> Create(Guid lessonId, string requestMessage = "", Lesson? newLesson = null)
    {
        var lesson = _context.Lessons
            .Include(x => x.Group).ThenInclude(x => x.LessonType)
            .FirstOrDefault(x => x.Id == lessonId) ?? throw new Exception("Lesson not found");

        //CheckValid 
        DateTime newStart = DateTime.Now;
        DateTime newEnd = DateTime.Now;
        if (newLesson != null)
        {
            var duration = (newLesson.EndTime - newLesson.StartTime).TotalMinutes;
            var defaultDuration = lesson.Group.LessonType.LessonTimeInMinutes;
            requestMessage += "\n| Default time to lesson type " + lesson.Group.Name + " : " + defaultDuration
                + "\n | New duration : " + duration
                + "\n Difference : " + (defaultDuration - duration);

            newStart = newLesson.StartTime;
            newEnd = newLesson.EndTime;

        }
        else
        {
            requestMessage += " in group '" + lesson.Group.Name + "' ";//+ "' lesson  was deleted by date:  ";
                   // + lesson.StartTime.ToShortTimeString() + " - " + lesson.EndTime.ToShortTimeString();
        }

        var user = await _userService.GetCurrentUserAsync();
        var request = new RequestLesson() { LessonBefore = lesson, RequestMessage = requestMessage, User = user, NewStart = newStart, NewEnd = newEnd };

        _context.Entry(request.User).State = EntityState.Unchanged;
        _context.Entry(request.LessonBefore).State = EntityState.Unchanged;

        _context.RequestLessons.Add(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<List<RequestLesson>> GetAll()
    {
        var items = await _context.RequestLessons
            .Include(x => x.User)
            .Include(x => x.LessonBefore).ThenInclude(x => x.Group)
            //.Include(x => x.LessonAfter).ThenInclude(x => x.Group)
            .Where(x => x.IsDeleted == false)
            .ToListAsync();
        return items;
    }

    public async Task<RequestLesson> Get(Guid id)
    {
        var item = await _context.RequestLessons
            .Include(x => x.User)
            .Include(x => x.LessonBefore).ThenInclude(x => x.Group)
            //.Include(x => x.LessonAfter).ThenInclude(x => x.Group)
            .FirstOrDefaultAsync(x => x.Id == id);
        return item ?? throw new Exception("Request not found");
    }

    //Надо отсортировать запросы.
    //А отправляет запрос на изменение. В принимает запрос. Изменение занятия. Запрос isDeleted = true, все остальные связанные с этим запросом - удаляются. Создается сообщение
    //А отправляет запрос на изменение. В не принимает запрос. Запрос isDeleted = true, все остальные связанные с этим запросом - удаляются. Создается сообщение
    //А отправляет запрос на удаление. 
    public async Task<RequestLesson> ApplyLessonRequest(RequestLesson item, int answer = 1)
    {
        item.IsDeleted = true;
        

        if (answer > 0)
        {
            var lastLesson = new Lesson() { StartTime = item.LessonBefore.StartTime, EndTime = item.LessonBefore.EndTime, GroupId = item.LessonBefore.GroupId };

            await DeletePreviousRequests(item);

            item.LessonBefore.StartTime = item.NewStart;
            item.LessonBefore.EndTime = item.NewEnd;

            _context.Lessons.Update(item.LessonBefore);

            await _notificationService.СreateToEditLesson(lastLesson, item.LessonBefore, null, answer);
            await _context.SaveChangesAsync();
            
        }
        
        _context.RequestLessons.Remove(item);
        
        await _context.SaveChangesAsync();
        return item;
    }
    public async Task<RequestLesson> ApplyLessonDeleteRequest(RequestLesson item, int answer = 1)
    {

        item.IsDeleted = true;

        if (answer > 0)
        {
            var lastLesson = new Lesson() { StartTime = item.LessonBefore.StartTime, EndTime = item.LessonBefore.EndTime, GroupId = item.LessonBefore.GroupId };

            await DeletePreviousRequests(item);

            var l = item.LessonBefore;

            _context.RequestLessons.Remove(item);

            await _context.SaveChangesAsync();
            _context.Lessons.Remove(l);

            await _notificationService.СreateToEditLesson(lastLesson, item.LessonBefore, null, answer);
            await _context.SaveChangesAsync();

        }
        else
        {
            _context.RequestLessons.Remove(item);
            
            await _context.SaveChangesAsync();
        }
        //await _notificationService.СreateToEditLesson(lastLessonValue,lesson, null,answer);
        return item;
    }

    public async Task DeletePreviousRequests(RequestLesson item)
    {
        var requestLessonsByLesson = await _context.RequestLessons
            .Include(x => x.LessonBefore)
            .Where(x=>x.LessonBefore.Id == item.LessonBefore.Id && x.Id != item.Id && !x.IsDeleted).ToListAsync();
        
        
        foreach (var i in requestLessonsByLesson)
        {
            i.LessonBefore = null;
            _context.Remove(i);
        }
        await _context.SaveChangesAsync();
        
    }

    public async Task DeletePreviousRequests(Lesson item)
    {
        var requestLessonsByLesson = await _context.RequestLessons
            .Include(x => x.LessonBefore).ThenInclude(x => x.Group)
            .Where(x => x.LessonBefore.Id == item.Id).ToListAsync();



        foreach (var i in requestLessonsByLesson)
        {
            i.LessonBefore = null;
            _context.Remove(i);
        }
        await _context.SaveChangesAsync();
    }


}
