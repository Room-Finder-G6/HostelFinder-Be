using AutoMapper;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.RoomAmenities.Response;
using HostelFinder.Application.DTOs.RoomDetails.Response;
using HostelFinder.Application.DTOs.ServiceCostDto.Responses;
using HostelFinder.Domain.Entities;
using HostelFinder.Application.DTOs.Users;
using HostelFinder.Application.DTOs.Users.Requests;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Mappings;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        // Room Mapping
        CreateMap<RoomResponseDto, Room>().ReverseMap()
            .ForMember(dest => dest.RoomTypeName,
                opt => opt.MapFrom(src => src.RoomType.TypeName))
            .ForMember(dest => dest.RoomDetailsDto,
                opt => opt.MapFrom(src => src.RoomDetails))
            .ForMember(dest=>dest.RoomAmenitiesDto,
                opt=>opt.MapFrom(src=>src.RoomAmenities))
            .ForMember(dest => dest.ServiceCostsDto,
                opt => opt.MapFrom(src => src.ServiceCosts))
            ;
        
        // RoomDetails Mapping
        CreateMap<RoomDetails, RoomDetailsResponseDto>().ReverseMap();
        
        // RoomAmenities Mapping
        CreateMap<RoomAmenities, RoomAmenitiesResponseDto>().ReverseMap();
        
        // Service Cost Mapping
        CreateMap<ServiceCost, ServiceCostResponseDto>().ReverseMap();
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