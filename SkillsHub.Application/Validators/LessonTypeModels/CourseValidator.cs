using FluentValidation;
using SkillsHub.Domain.Models;


namespace SkillsHub.Application.Validators.LessonTypeModels;

public class CourseValidator : AbstractValidator<Course>
{
    public CourseValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Направление обязательно");
        RuleFor(x => x.Subject).NotEmpty().WithMessage("Предмет обязателен");
        
        RuleFor(x => x)
           .Must(x => !x.IsVisibleOnMainPage || !string.IsNullOrEmpty(x.IdentityString))
           .WithMessage("Course can't be visible on page if identity not defined");

        RuleFor(x => x)
            .Must(x => !x.IsVisibleOnMainPage || (x.OrderOnMainPage > 0 && x.OrderOnMainPage < 50))
            .WithMessage("If Course is visible on the main page, OrderOnMainPage must be greater than 0 and less than 50");

    }
}

