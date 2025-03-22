using FluentValidation;
using SkillsHub.Application.DTO;
using System.ComponentModel.DataAnnotations;


namespace SkillsHub.Application.Validators;

public class UserCreateDTOValidator : AbstractValidator<UserCreateDTO>
{
    public UserCreateDTOValidator()
    {

        RuleFor(x => x.UserName).NotEmpty().WithMessage("Login is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");
        RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required.");
        RuleFor(x => x.PhonesArray).NotEmpty().WithMessage("Phone is required.").Must(x => x.Any(email => !string.IsNullOrEmpty(email))).WithMessage("Phone is required.").Must(x => x.All(email => new PhoneAttribute().IsValid(email))).WithMessage("Invalid phone format.");
        RuleFor(x => x.BirthDate.Year).LessThan((DateTime.Now).Year - 4).GreaterThan(1900).NotEmpty().WithMessage("Birthday not correct.");
        RuleFor(x => x)
            .Must(x => x.PasswordChanged == null || x.PasswordChangedConfirm == null || x.PasswordChanged == x.PasswordChangedConfirm)
            .WithMessage("PasswordChangedConfirm must be equal to PasswordChanged if PasswordChanged is not null.");


        //RuleFor(x => x.EmailsArray).NotEmpty().WithMessage("Email is required.").Must(x => x.Any(email => !string.IsNullOrEmpty(email))).WithMessage("Email is required.").Must(x => x.All(email => new EmailAddressAttribute().IsValid(email))).WithMessage("Invalid email format.");



        //if (item.EnglishLevelId != Guid.Empty) item.EnglishLevel = await _context.EnglishLevels.FirstOrDefaultAsync(x => x.Id == item.EnglishLevelId);
        //if (_context.ApplicationUsers.FirstOrDefault(x => x.Email == user.Email) != null || user.Email == null) throw new Exception("User with such email alredy exists");
        //if (_context.ApplicationUsers.FirstOrDefault(x => x.Phone == user.Phone) != null) throw new Exception("User with such phone alredy exists");


    }
}

