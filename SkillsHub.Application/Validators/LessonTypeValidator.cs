using FluentValidation;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;


namespace SkillsHub.Application.Validators;

public class LessonTypeValidator : AbstractValidator<LessonType>
{
    public LessonTypeValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.MinumumLessonsToPay).GreaterThan(0).WithMessage("MinumumLessonsToPay is 1");
		RuleFor(x => x.LessonTimeInMinutes).GreaterThan(0).WithMessage("Minimum LessonTimeInMinutes is 1");
		RuleFor(x => x.TeacherPrice).GreaterThan(0).WithMessage("Minimum TeacherPrice is 1");
		RuleFor(x => x.StudentPrice).GreaterThan(0).WithMessage("Minimum StudentPrice is 1");
		RuleFor(x => x.MinimumStudents).GreaterThan(0).WithMessage("Minimum Students is 1");
        RuleFor(x => x.MaximumStudents).LessThan(30).WithMessage("Maximum Students is 30");
        RuleFor(x=>x).Must(x=>x.MinimumStudents <= x.MaximumStudents).WithMessage("Maximum Students can`t be  less than minimum");
        //RuleFor(x => x.CourceId && x.CourseName).NotEmpty().WithMessage("Cource is required");
        //RuleFor(x => x.LessonType).NotEmpty().WithMessage("LessonType is required");
    }
}

