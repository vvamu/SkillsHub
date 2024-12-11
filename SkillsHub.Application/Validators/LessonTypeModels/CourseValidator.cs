using FluentValidation;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;


namespace SkillsHub.Application.Validators.LessonTypeModels;

public class CourseValidator : AbstractValidator<Course>
{
    public CourseValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Направление обязательно");
        RuleFor(x => x.Subject).NotEmpty().WithMessage("Предмет обязателен");

    }
}

