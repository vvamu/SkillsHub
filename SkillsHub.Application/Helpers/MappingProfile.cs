using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SkillsHub.Domain.Models;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Application.DTO;

namespace SkillsHub.Application.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<IdentityUser, ApplicationUser>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UserLoginDTO, ApplicationUser>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UserCreateDTO, ApplicationUser>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Teacher, TeacherDTO>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<ApplicationUser, TeacherDTO>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Student, StudentDTO>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<ApplicationUser, StudentDTO>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }

}


