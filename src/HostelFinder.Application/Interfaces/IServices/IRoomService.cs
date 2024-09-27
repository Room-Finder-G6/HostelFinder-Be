using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IRoomService
{
    Task<Response<RoomResponseDto>> GetAllRoomFeaturesByIdAsync(Guid roomId);
}