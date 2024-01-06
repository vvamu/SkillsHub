using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SkillsHub.Persistence;

namespace SkillsHub.Helpers;

public class InitMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly ApplicationDbContext _context;

    public InitMiddleware(ApplicationDbContext context,
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
		var emptyGroupName = "Users without group";
        var groupEmpty = await _context.Groups.FirstOrDefaultAsync(x => x.Name == emptyGroupName);
		if (groupEmpty != null) return;
		groupEmpty = new Group() { Name = emptyGroupName };
		await _context.Groups.AddAsync(groupEmpty);
		await _context.SaveChangesAsync();

		//await _context.Students.Include(x => x.Groups).Where(x => x.Groups != null && x.Groups.Count() > 0).ForEachAsync(x => x.Groups);

    }
}

