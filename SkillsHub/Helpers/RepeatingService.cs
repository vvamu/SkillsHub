﻿using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Persistence;

namespace SkillsHub.Helpers;

/*
public class CompleteLessonsMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly ApplicationDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CompleteLessonsMiddleware(ApplicationDbContext context, INotificationService notificationService,
		UserManager<ApplicationUser> userManager,
		RequestDelegate next,
		ILogger<ExceptionHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;

        _context = context;
		_notificationService = notificationService;
		_userManager = userManager;

    }

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception exception)
		{
			_logger.LogError(
				exception, "Exception occurred: {Message}", exception.Message);


			var problemDetails = new MyError
			{
				Status = StatusCodes.Status500InternalServerError,
				Title = "Server Error"
			};

			context.Response.StatusCode =
				StatusCodes.Status500InternalServerError;
			//return View("J")
			await context.Response.WriteAsJsonAsync(problemDetails);
		}
	}

	public async Task CheckEmptyGroup()
	{
		var emptyGroupName = "Users without group";
        var groupEmpty = await _context.Groups.FirstOrDefaultAsync(x => x.Name == emptyGroupName);
		if (groupEmpty != null) return;
		groupEmpty = new Group() { Name = emptyGroupName };
		await _context.Groups.AddAsync(groupEmpty);
		await _context.SaveChangesAsync();

		//await _context.Students.Include(x => x.Groups).Where(x => x.Groups != null && x.Groups.Count() > 0).ForEachAsync(x => x.Groups);

    }

   
}
*/
public class RepeatingService : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1));
    private readonly ILogger<RepeatingService> _logger; // Use Microsoft.Extensions.Logging.ILogger instead of Serilog.ILogger
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RepeatingService(ILogger<RepeatingService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        // _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CompleteLessons();
            await CreateLessonToUnlimitedGroup();

            await Task.Delay(60000 * 60, stoppingToken); //60000 = 1 min

        }
        if (stoppingToken.IsCancellationRequested)
        {

        }
    }



    public async Task CompleteLessons()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {

            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var _notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            _context.Groups.ToList();
            _context.Teachers.ToList();
            _context.Users.ToList();

            Lesson lessonR = null;
            foreach (var lesson in _context.Lessons.Where(x => x.IsDeleted == false))
                if (lesson.EndTime <= DateTime.Now.AddMinutes(30))
                {
                    lesson.IsСompleted = true;
                    lessonR = lesson;
                    _context.Lessons.Update(lesson);
                    await _context.SaveChangesAsync();

                }
            if (lessonR == null || lessonR.Group == null) return;

            List<ApplicationUser> usersToSend = new List<ApplicationUser>();
            var group = lessonR.Group;

            foreach (var teacher in group.GroupTeachers.Select(x => x.Teacher).Select(x => x.ApplicationUser))
            {
                usersToSend.Add(teacher);
                var message = "In group " + group.Name + " lesson by time " +
                    lessonR.StartTime.ToShortTimeString() + " - " + lessonR.EndTime.ToShortTimeString() + " " + lessonR.StartTime.ToShortDateString()
                    + "was completed automatically. Check what students marked.";

                await _notificationService.Create(message, usersToSend);
            }

            //var admins = _userManager.GetUsersInRoleAsync("Admin").Result; usersToSend.AddRange(admins);

        }
    }


    public async Task CreateLessonToUnlimitedGroup()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {

            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var _groupService = scope.ServiceProvider.GetRequiredService<IGroupService>();

            var groups = _groupService.GetItems().Where(x => x.IsUnlimitedLessonsCount);

            foreach (var group in groups)
            {
                if (group.Lessons.Where(x => x.IsСompleted == false).Count() < 10)
                {
                    DateTime DateStart = DateTime.Now;
                    if (group.Lessons != null && group.Lessons.Count != 0)
                    {
                        DateStart = group.Lessons.OrderByDescending(x => x.EndTime).FirstOrDefault().EndTime.AddDays(1); //Where(x=>x.EndTime <  DateTime.Now)
                    }

                    await _groupService.CreateLessonsBySchedule(group.DaySchedules, DateStart, 1, group, true);
                }
            }
        }
    }
}