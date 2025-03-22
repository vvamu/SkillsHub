using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

//For Admin Panel
public class AgeTypeService : AbstractLessonTypeLogModelService<AgeType>
{
    public AgeTypeService(ApplicationDbContext context, ILessonTypeService lessonTypeService) : base(context, lessonTypeService) { }

    protected override DbSet<AgeType> _contextModel => _context.AgeTypes;

    protected override IValidator<AgeType> _validator => new AgeTypeValidator();

    protected override IQueryable<AgeType>? _fullInclude => _context.AgeTypes;

    protected override void SetPropertyId(LessonType item, Guid value)
    {
        item.AgeTypeId = value;
    }
}
