using FluentValidation;
using SkillsHub.Application.DTO;

namespace SkillsHub.Application.Validators;

public class TeacherRegisterValidator : AbstractValidator<TeacherDTO>
{
    public TeacherRegisterValidator()
    {
        //RuleFor(x=>x.PossibleCources).NotEmpty().WithMessage("Teacher have to work at list with one cource`s type");
    }

}


