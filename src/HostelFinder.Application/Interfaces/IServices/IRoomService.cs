using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.Room.Responses;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IRoomService
    {
        Task<Response<List<RoomResponseDto>>> GetAllAsync();
        Task<Response<RoomResponseDto>> GetByIdAsync(Guid id);
        Task<Response<RoomResponseDto>> CreateAsync(AddRoomRequestDto roomDto);
        Task<Response<RoomResponseDto>> UpdateAsync(Guid id, UpdateRoomRequestDto roomDto);
        Task<Response<bool>> DeleteAsync(Guid id);
    }
}
