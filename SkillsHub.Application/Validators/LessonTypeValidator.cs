using FluentValidation;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;


namespace SkillsHub.Application.Validators;

/*
public class LessonTypeValidator : AbstractValidator<LessonType>
{
    public LessonTypeValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.MinumumLessonsToPay).GreaterThan(0).WithMessage("MinumumLessonsToPay is 1");
		RuleFor(x => x.LessonTimeInMinutes).GreaterThan(0).WithMessage("Minimum LessonTimeInMinutes is 1");
		
        //RuleFor(x => x.CourceId && x.CourseName).NotEmpty().WithMessage("Cource is required");
        //RuleFor(x => x.LessonType).NotEmpty().WithMessage("LessonType is required");
    }
}

*/