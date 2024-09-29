using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IRoomService
{
    Task<Response<RoomResponseDto>> GetAllRoomFeaturesByIdAsync(Guid roomId);
    Task<Response<AddRoomRequestDto>> AddRoomAsync(AddRoomRequestDto roomDto);
    Task<Response<UpdateRoomRequestDto>> UpdateRoomAsync(UpdateRoomRequestDto roomDto, Guid roomId);
}