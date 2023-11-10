

using FluentValidation;
using SkillsHub.Application.DTO;

namespace SkillsHub.Application.Validators;

public class LessonDTOValidator : AbstractValidator<LessonDTO>
{
    public LessonDTOValidator()
    {
        
        RuleFor(x => x.StartTime).NotEmpty().WithMessage("StartTime is required.");
        RuleFor(x => x.EndTime).NotEmpty().WithMessage("EndTime is required.");
        RuleFor(x => x.Teachers).NotEmpty().WithMessage("Teacher is required.");
        RuleFor(x => x.Group).NotEmpty().WithMessage("Group is required.");
        //RuleFor(x => x.LessonType).NotEmpty().WithMessage("LessonType is required.");

    }

}



