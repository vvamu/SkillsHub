using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SkillsHub.Persistence;

namespace SkillsHub.Helpers;

public class CheckLessonStatus
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly ApplicationDbContext _context;

    public CheckLessonStatus(ApplicationDbContext context,
		RequestDelegate next,
		ILogger<ExceptionHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;

        _context = context;

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
		var lessons = _context.Lessons;
		_context.Lessons.ToList();
		_context.Groups.ToList();
		foreach (var lesson in lessons)
		{
			if (lesson.EndTime < DateTime.Now && lesson.Group != null && !lesson.Group.IsLateDateStart)
			{
                lesson.IsСompleted = true;
            }
				
		}
    }
}

