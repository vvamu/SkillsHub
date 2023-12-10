using FluentValidation;
using SkillsHub.Application.DTO;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Validators;

public class StudentRegisterValidator : AbstractValidator<Student>
{
    public StudentRegisterValidator()
    {
        
    }

}


