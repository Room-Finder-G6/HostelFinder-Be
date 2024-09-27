using AutoMapper;
using HostelFinder.Application.DTOs.Room.Requests;
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
        var response = new Response<RoomResponseDto>(roomDto);
        return response;
    }

    public async Task<Response<AddRoomRequestDto>> AddRoomAsync(AddRoomRequestDto roomDto)
    {
        try
        {
            var roomDomain = _mapper.Map<Room>(roomDto);

            roomDomain.RoomDetails = _mapper.Map<RoomDetails>(roomDto.RoomDetails);
            roomDomain.RoomAmenities = _mapper.Map<RoomAmenities>(roomDto.RoomAmenities);
            roomDomain.ServiceCosts = _mapper.Map<List<ServiceCost>>(roomDto.ServiceCosts);
            var room = await _roomRepository.AddAsync(roomDomain);

            var roomResponseDto = _mapper.Map<AddRoomRequestDto>(room);
            return new Response<AddRoomRequestDto>(roomResponseDto);
        }
        catch (Exception ex)
        {
            return new Response<AddRoomRequestDto> { Succeeded = false, Errors = { ex.Message } };
        }  
    }

}