using FluentValidation;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;


namespace SkillsHub.Application.Validators;

public class LessonValidator : AbstractValidator<Lesson>
{
    public LessonValidator()
    {
        RuleFor(x => x.StartTime).NotEmpty().WithMessage("StartTime is required.");
        RuleFor(x => x.EndTime).NotEmpty().WithMessage("EndTime is required.");
        RuleFor(x => x).Must(x => x.Group != null || x.GroupId != Guid.Empty).WithMessage("Group is required.");

        RuleFor(x=>x).Must(x => x.StartTime < x.EndTime)
            .Must(x => x.StartTime != DateTime.MinValue && x.StartTime.Year > (DateTime.Now.Year - 10)  && x.StartTime.Year < (DateTime.Now.Year + 10))
            .Must(x => x.EndTime != DateTime.MaxValue && x.EndTime.Year > (DateTime.Now.Year - 10) && x.EndTime.Year < (DateTime.Now.Year + 10))
            .Must(x =>x.LessonTime > 10 && x.LessonTime < 200)
            .WithMessage("Not correct date");
        RuleFor(x => x)
            .Must(x => x.IsСompleted && x.ArrivedStudents.Count() > 0)
            .WithMessage("Group can't be started if not verified");
    }
}

