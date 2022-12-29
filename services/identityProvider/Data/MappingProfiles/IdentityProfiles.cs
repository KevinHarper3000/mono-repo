using AutoMapper;
using Data.Entities;
using Domain.Models;

namespace Data.MappingProfiles;

public class IdentityProfiles : Profile
{
    public IdentityProfiles()
    {
        CreateMap<User, UserViewModel>()
            .ReverseMap();
        CreateMap<User, UserViewModel>().ReverseMap();
    }
}