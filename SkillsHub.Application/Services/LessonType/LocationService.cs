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
public class LocationService : AbstractLessonTypeLogModelService<Location>
{
    protected override DbSet<Location> _contextModel => _context.Locations;

    protected override IValidator<Location> _validator => new LocationValidator();

    protected override IQueryable<Location>? _fullInclude => _context.Locations;
    public LocationService(ApplicationDbContext context, ILessonTypeService lessonTypeService) : base(context, lessonTypeService) { }
    protected override void SetPropertyId(LessonType item, Guid value)
    {
        item.LocationId = value;
    }
}
