using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

//For Admin Panel
public class GroupTypeService : AbstractLessonTypeLogModelService<GroupType> 
{
    public GroupTypeService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILessonTypeService lessonTypeService)
    {
        _context = context;
        _validator = new GroupTypeValidator();
        _contextModel = _context.GroupTypes;
        _lessonTypeService = lessonTypeService;
    }

    protected override void SetPropertyId(LessonType item, Guid value)
    {
       item.GroupTypeId = value;
    }
}
