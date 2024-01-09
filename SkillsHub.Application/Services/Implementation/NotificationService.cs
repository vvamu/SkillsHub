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
    private readonly IGroupService _groupService;

    public NotificationService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, IUserService userService, IGroupService groupService)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
        _userService = userService;
        _groupService = groupService;
    }
    


    public async Task<NotificationMessage> СreateToEditLesson(Lesson lastLessonValue, Lesson? lesson,  List<ApplicationUser>? usersToSend, int answer = 1)
    {
        //answer for: if Creator create request and his req was't apply send to him message about it
        NotificationMessage notification = new NotificationMessage();
        try
        {
            
            var group = await _groupService.GetAsync(lastLessonValue.GroupId ?? Guid.Empty);
            var notificationMessage = "";

            if (lesson == null)
            {
                notificationMessage += "In group " + group.Name + " lesson  was deleted by date:  "
                        + lastLessonValue.StartTime.ToShortTimeString() + " - " + lastLessonValue.EndTime.ToShortTimeString();
            }
            else
            {
                if (answer > 0)
                {

                    notificationMessage += "In group " + group.Name + " lesson  was changed from "
                        + lastLessonValue.StartTime.ToShortTimeString() + " - " + lastLessonValue.EndTime.ToShortTimeString()
                        + " to " + lesson.StartTime.ToShortTimeString() + " - " + lesson.EndTime.ToShortTimeString();

                }

                else
                {
                    notificationMessage += "In group " + group.Name + " lesson  was`s changed.";
                    usersToSend = new List<ApplicationUser>() { await _userService.GetCurrentUserAsync() };
                }
            }
            if (usersToSend == null)
            {
                usersToSend = new List<ApplicationUser>();

                var students = group.GroupStudents.Select(x=>x.Student).Select(x => x.ApplicationUser).ToList();
                var teacher = group.Teacher.ApplicationUser;
                var admins = _userManager.GetUsersInRoleAsync("Admin").Result;

                usersToSend.AddRange(students);
                usersToSend.Add(teacher);
                usersToSend.AddRange(admins);
            }

            notification = new NotificationMessage() { Message = notificationMessage };
            _context.NotificationMessages.Add(notification);
            await _context.SaveChangesAsync();
            foreach (var user in usersToSend)
            {
                var u = new ApplicationUser() { Id = user.Id };
                
                var r = new NotificationUser() { User = u, NotificationMessage = notification };
                _context.Entry(r.User).State = EntityState.Unchanged;
                _context.Entry(r.NotificationMessage).State = EntityState.Unchanged;
                await _context.NotificationUsers.AddAsync(r);


            }

            
            
            //_context.Entry(notification.Sender).State = EntityState.Unchanged;


            await _context.SaveChangesAsync();
        }
        catch { }

        return notification;

    }

    public async Task<NotificationMessage> СreateToUpdateCountLessonsInGroup(Group group, int previousCountLessons,int currentCountLessons, List<ApplicationUser>? usersToSend)
    {
        var message = "Count lessons was changed from " + previousCountLessons + " to " + currentCountLessons;
        var usersToSend2 = new List<NotificationUser>();
        if (usersToSend == null)
        {
            usersToSend = new List<ApplicationUser>();

            var students = group.GroupStudents.Select(x => x.Student).Select(x => x.ApplicationUser).ToList();
            var teacher = group.Teacher.ApplicationUser;
            var admins = _userManager.GetUsersInRoleAsync("Admin").Result;

            usersToSend.AddRange(students);
            usersToSend.Add(teacher);
            usersToSend.AddRange(admins);
        }

        var notification = new NotificationMessage() { Message = message };
        _context.NotificationMessages.Add(notification);


        foreach (var  user in usersToSend)
        {
            var r = new NotificationUser() { User = user, NotificationMessage= notification };
            _context.NotificationUsers.Add(r);

        }

        
        await _context.SaveChangesAsync();
        return notification;
    }
}
