using FluentValidation;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Validators;

public class TeacherValidator : AbstractValidator<Teacher>
{
    public TeacherValidator()
    {
        //RuleFor(x => x.PaymentAmount).GreaterThan(0);
        //RuleFor(x=>x.PossibleCources).NotEmpty().WithMessage("Teacher have to work at list with one cource`s type");
    }

}


