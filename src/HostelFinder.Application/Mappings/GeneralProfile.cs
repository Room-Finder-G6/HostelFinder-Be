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
using HostelFinder.Application.DTOs.Service.Request;
using HostelFinder.Application.DTOs.Service.Response;
using HostelFinder.Application.DTOs.Review.Request;
using HostelFinder.Application.DTOs.Review.Response;
using HostelFinder.Application.DTOs.Users.Response;

namespace HostelFinder.Application.Mappings;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        // Post Mapping
        CreateMap<Post, PostResponseDto>()
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

        CreateMap<AddPostRequestDto, Post>()
            .ForMember(dest => dest.Images,
                opt => opt.MapFrom(src => src.ImagesUrls.Select(url => new Image { Url = url })))
            .ForMember(dest => dest.RoomDetails,
                opt => opt.MapFrom(src => src.RoomDetails))
            .ForMember(dest => dest.ServiceCosts,
                opt => opt.MapFrom(src => src.ServiceCosts))
            .ReverseMap();

        CreateMap<Post, UpdatePostRequestDto>()
            .ForMember(dest => dest.UpdateRoomDetailsDto,
                opt => opt.MapFrom(src => src.RoomDetails))
            .ForMember(dest => dest.AddRoomAmenityDto,
                opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Post, ListPostResponseDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Hostel.Address))
            .ForMember(dest => dest.Size,
                opt => opt.MapFrom(src => src.Size))
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

        // Amenities Mapping
        CreateMap<Amenity, AmenityResponse>().ReverseMap();
        CreateMap<RoomAmenities, AddRoomAmenityDto>().ReverseMap();

        // Service Cost Mapping
        CreateMap<ServiceCost, ServiceCostResponseDto>().ReverseMap();
        CreateMap<ServiceCost, AddServiceCostDto>().ReverseMap();
        CreateMap<UpdateServiceCostDto, ServiceCost>();

        //Map User
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<UserProfileResponse, User>().ReverseMap();
        CreateMap<CreateUserRequestDto, User>().ReverseMap();

        //Service Mapping
        CreateMap<ServiceCreateRequestDTO, Service>();
        CreateMap<ServiceUpdateRequestDTO, Service>();
        CreateMap<Service, ServiceResponseDTO>();

        //Review Mapping
        CreateMap<AddReviewRequestDto, Review>();
        CreateMap<UpdateReviewRequestDto, Review>();
        CreateMap<Review, ReviewResponseDto>();
        CreateMap<Review, ReviewResponseDto>();
        CreateMap<User, LandlordResponseDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Username));
    }
}