using FluentValidation;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Validators;

public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        //RuleFor(x => x.PaymentAmount).GreaterThan(0);
    }

}


