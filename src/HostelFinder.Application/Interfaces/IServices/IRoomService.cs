using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.Room.Responses;
using HostelFinder.Application.Wrappers;
using Microsoft.AspNetCore.Http;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IRoomService
    {
        Task<Response<List<RoomResponseDto>>> GetAllAsync();
        Task<Response<RoomResponseDto>> GetByIdAsync(Guid id);
        Task<Response<RoomResponseDto>> CreateRoomAsync(AddRoomRequestDto roomDto, List<IFormFile> roomImages);
        Task<Response<RoomResponseDto>> UpdateAsync(Guid id, UpdateRoomRequestDto roomDto);
        Task<Response<bool>> DeleteAsync(Guid id);
        Task<Response<List<RoomResponseDto>>> GetRoomsByHostelIdAsync(Guid hostelId,int? floor);
    }
}
