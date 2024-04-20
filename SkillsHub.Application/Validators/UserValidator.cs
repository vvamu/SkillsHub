using FluentValidation;
using SkillsHub.Domain.BaseModels;


namespace SkillsHub.Application.Validators;

public class UserValidator : AbstractValidator<ApplicationUser>
{
    public UserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.Login).NotEmpty().WithMessage("Login is required.");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required.");
        RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required.");
        //RuleFor(x => x.BirthDate.Year).LessThan((DateTime.Now).Year - 3).GreaterThan(1900).NotEmpty().WithMessage("Birthday not correct.");


        //if (item.EnglishLevelId != Guid.Empty) item.EnglishLevel = await _context.EnglishLevels.FirstOrDefaultAsync(x => x.Id == item.EnglishLevelId);
        //if (_context.ApplicationUsers.FirstOrDefault(x => x.Email == user.Email) != null || user.Email == null) throw new Exception("User with such email alredy exists");
        //if (_context.ApplicationUsers.FirstOrDefault(x => x.Phone == user.Phone) != null) throw new Exception("User with such phone alredy exists");


    }
}

