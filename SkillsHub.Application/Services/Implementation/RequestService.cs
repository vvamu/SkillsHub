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

    public RequestService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, IUserService userService,
        INotificationService notificationService)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
        _userService = userService;
        _notificationService = notificationService;
    }
    


    public async Task<RequestLesson> Create(Guid lessonId, string requestMessage = "", Lesson? newLesson = null)
    {
        var lesson = _context.Lessons
            .Include(x => x.Group).ThenInclude(x => x.LessonType)
            .FirstOrDefault(x => x.Id == lessonId) ?? throw new Exception("Lesson not found");

        //CheckValid 
        if (newLesson != null)
        {
            var duration = (newLesson.EndTime - newLesson.StartTime).TotalMinutes;
            var defaultDuration = lesson.Group.LessonType.LessonTimeInMinutes;
            requestMessage += "\nDefault time to lesson type " + lesson.Group.LessonType.Name + " : " + defaultDuration
                + "\n New duration : " + duration
                + "\n Difference : " + (defaultDuration - duration);
        }
        else
        {
            requestMessage += "In group " + lesson.Group.Name + " lesson  was deleted by date:  "
                    + lesson.StartTime.ToShortTimeString() + " - " + lesson.EndTime.ToShortTimeString();
        }

        var user = await _userService.GetCurrentUserAsync();
        var request = new RequestLesson() { LessonBefore = lesson, RequestMessage = requestMessage, User = user };

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

    public async Task<Lesson> ApplyLessonRequest(RequestLesson item, Lesson lesson, int answer)
    {
        var lastLessonValue = await _context.Lessons.FirstOrDefaultAsync(x => x.Id == item.Id);


        var requestLessonsByLesson = await _context.RequestLessons
            .Include(x => x.LessonBefore).ThenInclude(x => x.Group)
            .Where(x => x.LessonBefore.Id == item.LessonBefore.Id).ToListAsync();

        if (requestLessonsByLesson  == null && requestLessonsByLesson.FirstOrDefault(x=>x.Id == item.Id) == null) return null;
        var requestLessonByLesson = requestLessonsByLesson.FirstOrDefault(x => x.Id == item.Id);

        requestLessonsByLesson.Remove(requestLessonByLesson);
        requestLessonsByLesson.ForEach(x=>_context.Remove(x));

        _context.RequestLessons.Update(requestLessonByLesson);
        requestLessonByLesson.IsDeleted = true;
        
        if (answer > 0)
        {
            _context.Lessons.Update(lesson);
        }
        await _notificationService.CreateToLesson(lastLessonValue,lesson, null,answer);
        await _context.SaveChangesAsync();
        return lesson;
    }

}
