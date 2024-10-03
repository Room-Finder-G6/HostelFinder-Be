using AutoMapper;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.RoomAmenities.Response;
using HostelFinder.Application.DTOs.RoomDetails.Response;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services;

public class RoomService : IRoomService
{
    private readonly IMapper _mapper;
    private readonly IRoomRepository _roomRepository;

    public RoomService(IMapper mapper, IRoomRepository roomRepository)
    {
        _mapper = mapper;
        _roomRepository = roomRepository;
    }

    public async Task<Response<RoomResponseDto>> GetAllRoomFeaturesByIdAsync(Guid roomId)
    {
        var room = await _roomRepository.GetAllRoomFeaturesByRoomId(roomId);
        if (room == null)
        {
            return new Response<RoomResponseDto>("Room not found");
        }

        var roomDto = _mapper.Map<RoomResponseDto>(room);
        roomDto.RoomAmenitiesDto = _mapper.Map<RoomAmenitiesResponseDto>(room.RoomAmenities);
        roomDto.RoomDetailsDto = _mapper.Map<RoomDetailsResponseDto>(room.RoomDetails);
        roomDto.ServiceCostsDto = _mapper.Map<List<ServiceCostResponseDto>>(room.ServiceCosts);

        var response = new Response<RoomResponseDto>(roomDto);
        return response;
    }

    public async Task<Response<AddRoomRequestDto>> AddRoomAsync(AddRoomRequestDto roomDto)
    {
        var roomDomain = _mapper.Map<Room>(roomDto);

        roomDomain.RoomDetails = _mapper.Map<RoomDetails>(roomDto.RoomDetails);
        roomDomain.RoomAmenities = _mapper.Map<RoomAmenities>(roomDto.RoomAmenities);
        roomDomain.ServiceCosts = _mapper.Map<List<ServiceCost>>(roomDto.ServiceCosts);
        roomDomain.CreatedOn = DateTime.Now;
        roomDomain.CreatedBy = "System";

        var room = await _roomRepository.AddAsync(roomDomain);
        var roomResponseDto = _mapper.Map<AddRoomRequestDto>(room);
        return new Response<AddRoomRequestDto>(roomResponseDto);
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
        _mapper.Map(roomDto.RoomDetails, existingRoom.RoomDetails);

        if (existingRoom.RoomAmenities == null)
        {
            existingRoom.RoomAmenities = new RoomAmenities { RoomId = roomId };
        }
        _mapper.Map(roomDto.RoomAmenities, existingRoom.RoomAmenities);

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
            return new Response<bool>{Succeeded = false, Message="Room not found"};
        }

        await _roomRepository.DeleteAsync(room.Id); 

        return new Response<bool> { Succeeded = true, Message = "Delete Room Successfully" };
    }

    public async Task<Response<List<ListRoomResponseDto>>> GetFilteredRooms(decimal? minPrice, decimal? maxPrice, string? location)
    {
        var rooms = await _roomRepository.GetFilteredRooms(minPrice, maxPrice, location);
        var roomsDto = _mapper.Map<List<ListRoomResponseDto>>(rooms);
        return new Response<List<ListRoomResponseDto>>(roomsDto);
    }
}