using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public NotificationService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, IUserService userService)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
        _userService = userService;
    }
    


    public async Task<NotificationMessage> СreateToEditLesson(Lesson lastLessonValue, Lesson? lesson,  List<ApplicationUser>? usersToSend, int answer = 1)
    {
        //answer for: if Creator create request and his req was't apply send to him message about it
        NotificationMessage notification = new NotificationMessage();
        try
        {
            
            var group = await GetAsync(lastLessonValue.GroupId ?? Guid.Empty);
            var notificationMessage = "";

            if (lesson == null)
            {
                notificationMessage += "In group " + group.Name + " lesson  was deleted by date:  "
                        + lastLessonValue.StartTime + " - " + lastLessonValue.EndTime;
            }
            else
            {
                if (answer > 0)
                {

                    notificationMessage += "In group " + group.Name + " lesson  was changed from "
                        + lastLessonValue.StartTime + " - " + lastLessonValue.EndTime
                        + " to " + lesson.StartTime + " - " + lesson.EndTime;

                }
                else
                {
                    notificationMessage += "In group " + group.Name + " lesson  was`s changed.";
                    usersToSend = new List<ApplicationUser>() { await _userService.GetCurrentUserAsync() };
                }
            }
            if (usersToSend == null) usersToSend = await GetUsersByGroup(group);
            

            var res = await Create(notificationMessage, usersToSend);
            return res;
           
        }
        catch { }

        return notification;

    }

    public async Task<NotificationMessage> СreateToUpdateCountLessonsInGroup(Group group, int previousCountLessons,int currentCountLessons, List<ApplicationUser>? usersToSend)
    {
        var message = "Count lessons was changed from " + previousCountLessons + " to " + currentCountLessons;
        if (usersToSend == null) usersToSend = await GetUsersByGroup(group);

        var res = await Create(message, usersToSend);
        return res;
    }

    private async Task<NotificationMessage> Create(NotificationMessage notification, List<ApplicationUser> usersToSend)
    {
        
        foreach (var user in usersToSend)
        {
            var u = new ApplicationUser() { Id = user.Id };
            var r = new NotificationUser() { User = u, NotificationMessage = notification };
            _context.Entry(r.User).State = EntityState.Unchanged;
            _context.Entry(r.NotificationMessage).State = EntityState.Unchanged;
            await _context.NotificationUsers.AddAsync(r);

        }
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task<NotificationMessage> Create(string message, List<ApplicationUser> usersToSend)
    {
        var admins = _userManager.GetUsersInRoleAsync("Admin").Result;
        if (usersToSend == null) usersToSend = admins.ToList();

        var notification = new NotificationMessage() { Message = message };

        await _context.NotificationMessages.AddAsync(notification);
        await _context.SaveChangesAsync();

        var res =  await Create(notification,usersToSend);
        return res;
    }

    public async Task<List<ApplicationUser>> GetUsersByGroup(Group group)
    {

        var usersToSend = new List<ApplicationUser>();

        var students = group.GroupStudents.Select(x => x.Student).Select(x => x.ApplicationUser).ToList();
        var teacher = group.Teacher.ApplicationUser;
        var admins = _userManager.GetUsersInRoleAsync("Admin").Result;

        usersToSend.AddRange(students);
        usersToSend.Add(teacher);
        usersToSend.AddRange(admins);
        return usersToSend;

    }

    public async Task<Group> GetAsync(Guid id)
    {
        var groups = await _context.Groups
            .Include(x => x.Lessons)
            .Include(x => x.Lessons).ThenInclude(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.Lessons).ThenInclude(x => x.ArrivedStudents).ThenInclude(x => x.Student).ThenInclude(x => x.ApplicationUser)


            .Include(x => x.Lessons).Include(x => x.CourseName).Include(x => x.LessonType)
            //.Include(x => x.GroupStudents).ThenInclude(x => x.Group)
            .Include(x => x.GroupStudents).ThenInclude(x => x.Student).ThenInclude(x => x.ApplicationUser)
            //.Include(x => x.GroupStudents).ThenInclude(x => x.Group).ThenInclude(x => x.Lessons)
            .Include(x => x.DaySchedules)
            .Include(x => x.Teacher).ThenInclude(x => x.ApplicationUser)
            .Include(x => x.CourseName)
            .Include(x => x.LessonType).ToListAsync();
        var item = groups.Find(x => x.Id == id);
        return item;
    }
}
