

using FluentValidation;
using SkillsHub.Application.DTO;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Validators;

public class LessonValidator : AbstractValidator<Lesson>
{
    public LessonValidator()
    {
        
        RuleFor(x => x.StartTime).NotEmpty().WithMessage("StartTime is required.");
        RuleFor(x => x.EndTime).NotEmpty().WithMessage("EndTime is required.");
        RuleFor(x => x.Teacher).NotEmpty().WithMessage("Teacher is required.");
        RuleFor(x => x.Group).NotEmpty().WithMessage("Group is required.");
        //RuleFor(x => x.LessonType).NotEmpty().WithMessage("LessonType is required.");

    }

}



