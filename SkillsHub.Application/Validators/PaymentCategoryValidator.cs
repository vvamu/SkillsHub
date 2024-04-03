using FluentValidation;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;


namespace SkillsHub.Application.Validators;

public class PaymentCategoryValidator : AbstractValidator<PaymentCategory>
{
    public PaymentCategoryValidator()
    {
        RuleFor(x => x.TeacherPrice).GreaterThan(0).WithMessage("Minimum TeacherPrice is 1");
        RuleFor(x => x.StudentPrice).GreaterThan(0).WithMessage("Minimum StudentPrice is 1");
        
        /*RuleFor(x => x.MinCountStudents).GreaterThan(0).WithMessage("Minimum Students can`t be less than 1");
        RuleFor(x => x.MaxCountStudents).LessThan(30).WithMessage("Maximum Students can`t be more than 30");
        RuleFor(x => x).Must(x => x.MinCountStudents <= x.MaxCountStudents).WithMessage("Maximum Students can`t be  less than minimum");*/

    }
}

