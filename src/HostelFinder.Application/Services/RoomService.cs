using AutoMapper;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;

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

    public Task<Response<RoomResponseDto>> GetAllRoomFeaturesByIdAsync(Guid roomId)
    {
        var room = _roomRepository.GetAllRoomFeaturesByRoomId(roomId);
        var roomDto = _mapper.Map<RoomResponseDto>(room);
        var response = new Response<RoomResponseDto>(roomDto);
        return Task.FromResult(response);
    }
}