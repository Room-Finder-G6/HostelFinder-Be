﻿using AutoMapper;
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
using HostelFinder.Application.DTOs.Users.Response;
using HostelFinder.Application.DTOs.Membership.Responses;
using HostelFinder.Application.DTOs.Membership.Requests;
using HostelFinder.Application.DTOs.MembershipService.Requests;
using HostelFinder.Application.DTOs.MembershipService.Responses;
using HostelFinder.Application.Wrappers;
using HostelFinder.Application.DTOs.Image.Responses;
using HostelFinder.Application.DTOs.InVoice.Responses;
using HostelFinder.Application.DTOs.InVoice.Requests;
using HostelFinder.Application.DTOs.Room.Responses;

namespace HostelFinder.Application.Mappings;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        // Post Mapping
        CreateMap<Post, PostResponseDto>()
            .ForMember(dest => dest.ImageUrls,
                opt => opt.MapFrom(src => src.Images.Select(x => x.Url).ToList()))
            .ReverseMap()
            ;

        CreateMap<AddPostRequestDto, Post>()
            .ForMember(dest => dest.Images,
                opt => opt.MapFrom(src => src.ImagesUrls.Select(url => new Image { Url = url })))
            .ReverseMap();

        CreateMap<Post, UpdatePostRequestDto>()
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
            .ForMember(dest => dest.Image,
                opt => opt.MapFrom(src => src.Images))
            .ReverseMap();

        // Hostel Mapping
        CreateMap<Hostel, HostelResponseDto>().ReverseMap();
        CreateMap<Hostel, AddHostelRequestDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ReverseMap();

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
        CreateMap<AddAmenityDto, Amenity>().ReverseMap();

        // Service Cost Mapping
        CreateMap<ServiceCost, ServiceCostResponseDto>().ReverseMap();
        CreateMap<ServiceCost, AddServiceCostDto>().ReverseMap();
        CreateMap<UpdateServiceCostDto, ServiceCost>();

        //Map User
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserProfileResponse, User>().ReverseMap();
        CreateMap<CreateUserRequestDto, User>().ReverseMap();
        
        CreateMap<UserProfileResponse, Response<UserProfileResponse>>()
           .ConstructUsing(src => new Response<UserProfileResponse>
           {
               Data = src,
               Succeeded = true
           });

        //Service Mapping
        CreateMap<ServiceCreateRequestDTO, Service>();
        CreateMap<ServiceUpdateRequestDTO, Service>();
        CreateMap<Service, ServiceResponseDTO>();

        //Membership
        CreateMap<Membership, MembershipResponseDto>().ReverseMap();
        CreateMap<MembershipServiceResponseDto, AddMembershipServiceReqDto>().ReverseMap();
        CreateMap<AddMembershipRequestDto, Membership>().ReverseMap();
        CreateMap<AddMembershipServiceReqDto, MembershipServices>()
           .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceName)).ReverseMap();
        CreateMap<UpdateMembershipRequestDto, Membership>().ReverseMap();
        CreateMap<MembershipServices, MembershipServiceResponseDto>()
           .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceName)).ReverseMap();
        CreateMap<Membership, MembershipResponseDto>()
            .ForMember(dest => dest.MembershipServices, opt => opt.MapFrom(src => src.MembershipServices)).ReverseMap();
        CreateMap<List<Membership>, Response<List<MembershipResponseDto>>>()
           .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src));


        //Image
        CreateMap<Image, ImageResponseDto>().ReverseMap();

        //InVoice
        CreateMap<Invoice, InvoiceResponseDto>().ReverseMap();
        CreateMap<UpdateInvoiceRequestDto, Invoice>().ReverseMap();
        CreateMap<AddInVoiceRequestDto, Invoice>().ReverseMap();

        //Room
        CreateMap<Room, RoomResponseDto>().ReverseMap();
        CreateMap<UpdateRoomRequestDto, Room>().ReverseMap();
        CreateMap<AddRoomRequestDto, Room>().ReverseMap();
    }
}