using FluentValidation;
using SkillsHub.Application.DTO;

namespace SkillsHub.Application.Validators;

public class StudentRegisterValidator : AbstractValidator<StudentDTO>
{
    public StudentRegisterValidator()
    {
        RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.");

        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Password is required.");

        RuleFor(x => x.FirstName)
        .NotEmpty().WithMessage("FirstName is required.");

        RuleFor(x => x.LastName)
        .NotEmpty().WithMessage("FirstName is required.");

        RuleFor(x => x.Login)
        .NotEmpty().WithMessage("Login is required.");
    }

}


