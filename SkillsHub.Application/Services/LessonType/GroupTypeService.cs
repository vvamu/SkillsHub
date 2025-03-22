using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Application.Validators.LessonTypeModels;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

//For Admin Panel
public class GroupTypeService : AbstractLessonTypeLogModelService<GroupType>
{
    protected override DbSet<GroupType> _contextModel => _context.GroupTypes;

    protected override IValidator<GroupType> _validator => new GroupTypeValidator();

    protected override IQueryable<GroupType>? _fullInclude => _context.GroupTypes;

    public GroupTypeService(ApplicationDbContext context, ILessonTypeService lessonTypeService) : base(context, lessonTypeService) { }

    protected override void SetPropertyId(LessonType item, Guid value)
    {
        item.GroupTypeId = value;
    }
}
