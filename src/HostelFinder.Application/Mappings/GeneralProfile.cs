using AutoMapper;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.RoomAmenities.Response;
using HostelFinder.Application.DTOs.RoomDetails.Response;
using HostelFinder.Application.DTOs.ServiceCost.Request;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Domain.Entities;
using HostelFinder.Application.DTOs.Users;
using HostelFinder.Application.DTOs.Users.Requests;
using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.DTOs.RoomAmenities.Request;
using HostelFinder.Application.DTOs.RoomDetails.Request;

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
            .ForMember(dest => dest.RoomAmenitiesDto,
                opt => opt.MapFrom(src => src.RoomAmenities))
            .ForMember(dest => dest.ServiceCostsDto,
                opt => opt.MapFrom(src => src.ServiceCosts))
            .ReverseMap()
            ;
        
        CreateMap<AddRoomRequestDto, Room>()
            .ForMember(dest => dest.Images, 
                opt => opt.MapFrom(src => src.ImagesUrls.
                    Select(url => new Image { Url = url })))
            .ForMember(dest => dest.RoomDetails,
                opt => opt.MapFrom(src => src.RoomDetails))
            .ForMember(dest => dest.RoomAmenities,
                opt => opt.MapFrom(src => src.RoomAmenities))
            .ForMember(dest => dest.ServiceCosts,
                opt => opt.MapFrom(src => src.ServiceCosts))
            .ReverseMap();
     
        /*CreateMap<Room, AddRoomRequestDto>()
            .ForMember(dest => dest.ImagesUrls, 
                opt => opt.MapFrom(src => src.Images.Select(image => image.Url)))
            .ForMember(dest => dest.RoomDetails, opt 
                => opt.MapFrom(src => src.RoomDetails))
            .ForMember(dest => dest.RoomAmenities, opt 
                => opt.MapFrom(src => src.RoomAmenities))
            .ForMember(dest => dest.ServiceCosts, opt 
                => opt.MapFrom(src => src.ServiceCosts));*/

        CreateMap<Room, UpdateRoomRequestDto>()
            .ForMember(dest => dest.RoomAmenities,
                opt => opt.MapFrom(src => src.RoomAmenities))
            .ForMember(dest => dest.RoomDetails,
                opt => opt.MapFrom(src => src.RoomDetails))
            .ReverseMap();

        // Hostel Mapping
        CreateMap<Hostel, HostelResponseDto>().ReverseMap();
        CreateMap<Hostel, AddHostelRequestDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ReverseMap();
        CreateMap<Hostel, HostelResponseDto>()
           .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Reviews.Any() ? src.Reviews.Average(r => r.rating) : 0))
           .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images.ToString()));

        CreateMap<Hostel, ListHostelResponseDto>()
            .ForMember(dest => dest.LandlordUserName, opt => opt.MapFrom(src => src.Landlord.Username))
            .ReverseMap();

        // Address Mapping
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<Hostel, UpdateHostelRequestDto>().ReverseMap();

        // RoomDetails Mapping
        CreateMap<RoomDetails, RoomDetailsResponseDto>().ReverseMap();
        CreateMap<RoomDetails, AddRoomDetailsDto>().ReverseMap();
        CreateMap<RoomDetails, UpdateRoomDetailsDto>().ReverseMap();

        // RoomAmenities Mapping
        CreateMap<RoomAmenities, RoomAmenitiesResponseDto>().ReverseMap();
        CreateMap<RoomAmenities, AddRoomAmenitiesDto>().ReverseMap();
        CreateMap<RoomAmenities, UpdateRoomAmenitiesDto>().ReverseMap();

        // Service Cost Mapping
        CreateMap<ServiceCost, ServiceCostResponseDto>().ReverseMap();
        CreateMap<ServiceCost, AddServiceCostDto>().ReverseMap();

        //Map User
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<CreateUserRequestDto, User>().ReverseMap();
    }
}