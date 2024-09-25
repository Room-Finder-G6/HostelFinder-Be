using AutoMapper;
using HostelFinder.Application.DTOs.Users;
using HostelFinder.Application.DTOs.Users.Requests;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {

            //Map User
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<CreateUserRequestDto, User>().ReverseMap();
        }
    }
}