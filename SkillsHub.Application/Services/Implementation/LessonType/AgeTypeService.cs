using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Application.Validators.LessonTypeModels;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

//For Admin Panel
public class AgeTypeService : AbstractLessonTypeLogModelService<AgeType> 
{
    public AgeTypeService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILessonTypeService lessonTypeService)
    {
        _context = context;
        _validator = new AgeTypeValidator();
        _contextModel = _context.AgeTypes;
        _lessonTypeService = lessonTypeService;
    }

    protected override void SetPropertyId(LessonType item, Guid value)
    {
       item.AgeTypeId = value;
    }
}
