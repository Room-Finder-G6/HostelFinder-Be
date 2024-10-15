using AutoMapper;
using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.RoomDetails.Response;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace HostelFinder.Application.Services;

public class RoomService : IRoomService
{
    private readonly IMapper _mapper;
    private readonly IRoomRepository _roomRepository;
    private readonly IS3Service _s3Service;

    public RoomService(IMapper mapper, IRoomRepository roomRepository, IS3Service s3Service)
    {
        _mapper = mapper;
        _roomRepository = roomRepository;
        _s3Service = s3Service;
    }

    public async Task<Response<RoomResponseDto>> GetAllRoomFeaturesByIdAsync(Guid roomId)
    {
        var room = await _roomRepository.GetAllRoomFeaturesByRoomId(roomId);
        if (room == null)
        {
            return new Response<RoomResponseDto>("Room not found");
        }

        var roomDto = _mapper.Map<RoomResponseDto>(room);
        roomDto.RoomDetailsDto = _mapper.Map<RoomDetailsResponseDto>(room.RoomDetails);
        roomDto.ServiceCostsDto = _mapper.Map<List<ServiceCostResponseDto>>(room.ServiceCosts);

        var response = new Response<RoomResponseDto>(roomDto);
        return response;
    }

    public async Task<Response<AddRoomRequestDto>> AddRoomAsync(AddRoomRequestDto roomDto)
    {
        try
        {
            if (roomDto.PrimaryImage == null)
            {
                throw new ArgumentException("Primary image is required.");
            }

            // Upload primary image to S3
            var primaryImageUrl = await _s3Service.UploadFileAsync(roomDto.PrimaryImage);

            // Upload other images (if any)
            var images = new List<Image>();
            if (roomDto.Images != null && roomDto.Images.Length > 0)
            {
                var uploadTasks = roomDto.Images.Select(file => _s3Service.UploadFileAsync(file));
                var urls = await Task.WhenAll(uploadTasks);
                images.AddRange(urls.Select(url => new Image { Url = url }));
            }

            // Map DTO to domain after uploading images
            var roomDomain = _mapper.Map<Room>(roomDto);
            roomDomain.PrimaryImageUrl = primaryImageUrl;
            roomDomain.Images = images;

            // Save room information to database
            roomDomain = await _roomRepository.AddAsync(roomDomain);

            // Add amenities for the room
            await AddRoomAmenitiesAsync(roomDto.AddRoomAmenity, roomDomain.Id);

            // Map back from domain to DTO
            var roomResponseDto = _mapper.Map<AddRoomRequestDto>(roomDomain);
            return new Response<AddRoomRequestDto>(roomResponseDto);
        }
        catch (ArgumentException ex)
        {
            // Handle validation errors
            return new Response<AddRoomRequestDto>(ex.Message);
        }
        catch (Exception ex)
        {
            return new Response<AddRoomRequestDto>("An error occurred while adding the room. Please try again.");
        }
    }


    private async Task AddRoomAmenitiesAsync(List<AddRoomAmenityDto> amenities, Guid roomId)
    {
        foreach (var amenityDto in amenities)
        {
            if (amenityDto.IsSelected)
            {
                var roomAmenity = new RoomAmenities
                {
                    RoomId = roomId,
                    AmenityId = amenityDto.Id
                };

                await _roomRepository.AddRoomAmenitiesAsync(roomAmenity);
            }
        }
    }

    public async Task<Response<UpdateRoomRequestDto>> UpdateRoomAsync(UpdateRoomRequestDto roomDto, Guid roomId)
    {
        var existingRoom = await _roomRepository.GetAllRoomFeaturesByRoomId(roomId);

        if (existingRoom == null)
        {
            return new Response<UpdateRoomRequestDto>("Room not found");
        }

        _mapper.Map(roomDto, existingRoom);

        if (existingRoom.RoomDetails == null)
        {
            existingRoom.RoomDetails = new RoomDetails { RoomId = roomId };
        }

        _mapper.Map(roomDto.UpdateRoomDetailsDto, existingRoom.RoomDetails);

        _mapper.Map(roomDto.AddRoomAmenityDto, existingRoom.RoomAmenities);

        /*if (roomDto.ServiceCosts != null)
        {
            existingRoom.ServiceCosts = _mapper.Map<List<ServiceCost>>(roomDto.ServiceCosts);
        }*/

        existingRoom.LastModifiedOn = DateTime.Now;
        existingRoom.LastModifiedBy = "System";

        await _roomRepository.UpdateAsync(existingRoom);
        var roomResponseDto = _mapper.Map<UpdateRoomRequestDto>(existingRoom);
        return new Response<UpdateRoomRequestDto>(roomResponseDto);
    }

    public async Task<Response<bool>> DeleteRoomAsync(Guid roomId)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null)
        {
            return new Response<bool> { Succeeded = false, Message = "Room not found" };
        }

        await _roomRepository.DeleteAsync(room.Id);

        return new Response<bool> { Succeeded = true, Message = "Delete Room Successfully" };
    }

    public async Task<Response<List<ListRoomResponseDto>>> GetFilteredRooms(decimal? minPrice, decimal? maxPrice,
        string? location, RoomType roomType)
    {
        var rooms = await _roomRepository.GetFilteredRooms(minPrice, maxPrice, location, roomType);
        var roomsDto = _mapper.Map<List<ListRoomResponseDto>>(rooms);
        return new Response<List<ListRoomResponseDto>>(roomsDto);
    }
}