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
            requestMessage += "\n| Default time to lesson type " + lesson.Group.LessonType.Name + " : " + defaultDuration
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
    public async Task<Lesson> ApplyLessonRequest(RequestLesson item, Lesson lesson, int answer = 1)
    {
        //var req =  await _context.RequestLessons.Include(x => x.LessonBefore).FirstOrDefaultAsync(x=>x.Id  ==item.Id);
        //var lastLessonValue =  req.LessonBefore;
        //var group = await _context.Groups.Include(x => x.Lessons).FirstOrDefaultAsync(x => x.Lessons.Select(x => x.Id).Contains(lastLessonValue.Id));

        //full requestLesson


        if (answer > 0)
        {
            item.IsDeleted = true;

            _context.RequestLessons.Update(item);

            await DeletePreviousRequests(item);

            item.LessonBefore.StartTime = item.NewStart;
            item.LessonBefore.EndTime = item.NewEnd;

            _context.Lessons.Update(item.LessonBefore);
            await _context.SaveChangesAsync();
            
        }

        //await _notificationService.СreateToEditLesson(lastLessonValue,lesson, null,answer);
        await _context.SaveChangesAsync();
        return lesson;
    }

    public async Task DeletePreviousRequests(RequestLesson item)
    {
        var requestLessonsByLesson = await _context.RequestLessons
            .Include(x => x.LessonBefore)
            .Where(x=>x.LessonBefore.Id == item.LessonBefore.Id && x.Id != item.Id && !x.IsDeleted).ToListAsync();
        
        
        foreach (var i in requestLessonsByLesson)
        {

            _context.Entry(i.LessonBefore).State = EntityState.Unchanged;
            if (i.Id == item.Id) continue;
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
            _context.Remove(i.Id);
        }
        await _context.SaveChangesAsync();
    }


    }
