using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SkillsHub.Domain.DTO;
using SkillsHub.Domain.Models;
using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Application.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<IdentityUser, ApplicationUser>().ReverseMap()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UserLogin, ApplicationUser>().ReverseMap()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    }

}


