using FluentValidation;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;


namespace SkillsHub.Application.Validators;

public class GroupValidator : AbstractValidator<Group>
{
    public GroupValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x).Must(x => x.CourseName != null || x.CourseNameId != Guid.Empty).WithMessage("Cource not found");
        RuleFor(x => x).Must(x => x.LessonType != null || x.LessonTypeId != Guid.Empty).WithMessage("Lesson type not found");


        RuleFor(x => x)
            .Must(x => (x.IsUnlimitedLessonsCount && x.LessonsCount <= 0) || (!x.IsUnlimitedLessonsCount && x.LessonsCount > 1))
            .WithMessage("Invalid lessons сount value");

        RuleFor(x => x)
            .Must(x => (!x.IsLateDateStart && (x.DateStart.Year >= DateTime.Now.Year - 1 && x.DateStart.Year <= DateTime.Now.Year + 1)) 
            || (x.IsLateDateStart))
            .WithMessage("Not valid date start");

        RuleFor(x => x)
             .Must(x => (!x.IsVerified && x.DateStart > DateTime.Now) || (x.IsVerified))
             .WithMessage("Group can`t be started if not verified. Choose a later start time.");

        

        //RuleFor(x => x.CourceId && x.CourseName).NotEmpty().WithMessage("Cource is required");
        //RuleFor(x => x.LessonType).NotEmpty().WithMessage("LessonType is required");
    }
}

