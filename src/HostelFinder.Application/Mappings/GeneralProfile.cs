using AutoMapper;
using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.DTOs.Amenity.Response;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.RoomDetails.Response;
using HostelFinder.Application.DTOs.ServiceCost.Request;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Domain.Entities;
using HostelFinder.Application.DTOs.Users;
using HostelFinder.Application.DTOs.Users.Requests;
using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.DTOs.RoomDetails.Request;
using HostelFinder.Application.DTOs.Address;

namespace HostelFinder.Application.Mappings;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        // Room Mapping
        CreateMap<Room, RoomResponseDto>()
            .ForMember(dest => dest.ImageUrls,
                opt => opt.MapFrom(src => src.Images.Select(x => x.Url).ToList()))
            .ForMember(dest => dest.RoomDetailsDto,
                opt => opt.MapFrom(src => src.RoomDetails))
            .ForMember(dest => dest.AmenityResponses,
                opt => opt.MapFrom(src => src.RoomAmenities.Select(ra => new AmenityResponse
                {
                    AmenityName = ra.Amenity.AmenityName,
                    IsSelected = ra.Amenity.IsSelected
                })))
            .ForMember(dest => dest.ServiceCostsDto,
                opt => opt.MapFrom(src => src.ServiceCosts))
            .ReverseMap()
            ;

        CreateMap<AddRoomRequestDto, Room>()
            .ForMember(dest => dest.Images,
                opt => opt.MapFrom(src => src.ImagesUrls.Select(url => new Image { Url = url })))
            .ForMember(dest => dest.RoomDetails,
                opt => opt.MapFrom(src => src.RoomDetails))
            .ForMember(dest => dest.ServiceCosts,
                opt => opt.MapFrom(src => src.ServiceCosts))
            .ReverseMap();

        CreateMap<Room, UpdateRoomRequestDto>()
            .ForMember(dest => dest.UpdateRoomDetailsDto,
                opt => opt.MapFrom(src => src.RoomDetails))
            .ForMember(dest => dest.AddRoomAmenityDto,
                opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Room, ListRoomResponseDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Hostel.Address))
            .ForMember(dest => dest.Size,
                opt => opt.MapFrom(src => src.RoomDetails.Size))
            .ForMember(dest => dest.PrimaryImageUrl,
                opt => opt.MapFrom(src => src.PrimaryImageUrl))
            .ForMember(dest => dest.MonthlyRentCost,
                opt => opt.MapFrom(src => src.MonthlyRentCost))
            .ReverseMap();

        // Hostel Mapping
        CreateMap<Hostel, HostelResponseDto>().ReverseMap();
        CreateMap<Hostel, AddHostelRequestDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ReverseMap();

        // Address Mapping
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<Hostel, UpdateHostelRequestDto>().ReverseMap();

        // RoomDetails Mapping
        CreateMap<RoomDetails, RoomDetailsResponseDto>().ReverseMap();
        CreateMap<RoomDetails, AddRoomDetailsDto>().ReverseMap();
        CreateMap<RoomDetails, UpdateRoomDetailsDto>().ReverseMap();

        // Amenities Mapping
        CreateMap<Amenity, AmenityResponse>().ReverseMap();
        CreateMap<RoomAmenities, AddRoomAmenityDto>().ReverseMap();

        // Service Cost Mapping
        CreateMap<ServiceCost, ServiceCostResponseDto>().ReverseMap();
        CreateMap<ServiceCost, AddServiceCostDto>().ReverseMap();

        //Map User
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<CreateUserRequestDto, User>().ReverseMap();
    }
}