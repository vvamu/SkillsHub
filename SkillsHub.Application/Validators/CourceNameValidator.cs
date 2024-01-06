using FluentValidation;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;


namespace SkillsHub.Application.Validators;

public class CourceNameValidator : AbstractValidator<CourseName>
{
    public CourceNameValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.MinimumAge).GreaterThan(0).WithMessage("Minimum Age must be at least is 1");
		RuleFor(x => x.MaximumAge).GreaterThan(0).WithMessage("Maximum Age must be at least is 1");
		//RuleFor(x => x.CourceId && x.CourseName).NotEmpty().WithMessage("Cource is required");
		//RuleFor(x => x.LessonType).NotEmpty().WithMessage("LessonType is required");
	}
}

