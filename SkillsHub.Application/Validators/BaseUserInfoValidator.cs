using FluentValidation;
using SkillsHub.Domain.BaseModels;


namespace SkillsHub.Application.Validators;

public class BaseUserInfoValidator : AbstractValidator<BaseUserInfo>
{
    public BaseUserInfoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required.");
        RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required.");
        RuleFor(x => x.BirthDate.Year).LessThan((DateTime.Now).Year - 2).GreaterThan(1900).NotEmpty().WithMessage("Birthday not correct.");
    }
}

