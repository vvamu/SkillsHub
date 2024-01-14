using FluentValidation;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Validators;

public class TeacherRegisterValidator : AbstractValidator<Teacher>
{
    public TeacherRegisterValidator()
    {
        //RuleFor(x=>x.PossibleCources).NotEmpty().WithMessage("Teacher have to work at list with one cource`s type");
    }

}


