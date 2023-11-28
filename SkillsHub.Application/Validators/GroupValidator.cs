using FluentValidation;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;


namespace SkillsHub.Application.Validators;

public class GroupValidator : AbstractValidator<Group>
{
    public GroupValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        //RuleFor(x => x.CourceId && x.CourceName).NotEmpty().WithMessage("Cource is required");
        //RuleFor(x => x.LessonType).NotEmpty().WithMessage("LessonType is required");
    }
}

