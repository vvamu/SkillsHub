using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Domain.Models.NotInUse;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

//For Admin Panel
public class LessonTypeService : ILessonTypeService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public LessonTypeService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    /*
    public async Task<LessonType> CreateLessonType(LessonType item)
    {
		if (item == null) throw new Exception("Not correct data for group");
		var userRegisterValidator = new LessonTypeValidator();
		var validationResult = await userRegisterValidator.ValidateAsync(item);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors;
			var errorsString = string.Concat(errors);
			throw new Exception(errorsString);
		}

		if (_context.LessonTypes.FirstOrDefault(x => x.Name == item.Name) != null) throw new Exception("Lesson type with such name already exist");
        item.DateCreated = DateTime.Now;
        await _context.LessonTypes.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;
	}
    */
}
