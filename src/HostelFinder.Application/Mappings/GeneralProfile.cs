using AutoMapper;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.RoomAmenities.Response;
using HostelFinder.Application.DTOs.RoomDetails.Response;
using HostelFinder.Application.DTOs.ServiceCost.Request;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Domain.Entities;
using HostelFinder.Application.DTOs.Users;
using HostelFinder.Application.DTOs.Users.Requests;

namespace HostelFinder.Application.Mappings;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        // Room Mapping
        CreateMap<RoomResponseDto, Room>().ReverseMap()
            .ForMember(dest => dest.ImageUrls,
                opt => opt.MapFrom(src => src.Images.Select(x => x.Url).ToList()))
            .ForMember(dest => dest.RoomDetailsDto,
                opt => opt.MapFrom(src => src.RoomDetails))
            .ForMember(dest => dest.RoomAmenitiesDto,
                opt => opt.MapFrom(src => src.RoomAmenities))
            .ForMember(dest => dest.ServiceCostsDto,
                opt => opt.MapFrom(src => src.ServiceCosts))
            ;

        CreateMap<AddRoomRequestDto, Room>().ReverseMap()
            .ForMember(dest => dest.ImageUrls,
                opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.RoomDetails,
                opt => opt.MapFrom(src => src.RoomDetails))
            .ForMember(dest => dest.RoomAmenities,
                opt => opt.MapFrom(src => src.RoomAmenities))
            .ForMember(dest => dest.ServiceCosts,
                opt => opt.MapFrom(src => src.ServiceCosts))
            ;

        // RoomDetails Mapping
        CreateMap<RoomDetails, RoomDetailsResponseDto>().ReverseMap();
        CreateMap<RoomDetails, AddRoomRequestDto>().ReverseMap();

        // RoomAmenities Mapping
        CreateMap<RoomAmenities, RoomAmenitiesResponseDto>().ReverseMap();
        CreateMap<RoomAmenities, RoomAmenitiesResponseDto>().ReverseMap();

        // Service Cost Mapping
        CreateMap<ServiceCost, ServiceCostResponseDto>().ReverseMap();
        CreateMap<ServiceCost, AddServiceCostDto>().ReverseMap();

        //Map User
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<CreateUserRequestDto, User>().ReverseMap();
    }
}