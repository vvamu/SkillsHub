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

        CreateMap<ApplicationUser, Teacher>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<ApplicationUser, Student>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        CreateMap<Student, LessonStudent>()
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src));
        /*
        CreateMap<LessonStudent, Student>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentId))
            .ForMember(dest => dest, opt => opt.MapFrom(src => src.Student));
        */
    }

}


