using FluentValidation;
using SkillsHub.Application.DTO;

namespace SkillsHub.Application.Validators;

public class UserLoginValidator : AbstractValidator<UserLoginDTO>
{
    public UserLoginValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Login is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }

}


