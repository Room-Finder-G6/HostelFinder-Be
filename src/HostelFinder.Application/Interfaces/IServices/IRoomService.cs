using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Wrappers;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IRoomService
{
    Task<Response<RoomResponseDto>> GetAllRoomFeaturesByIdAsync(Guid roomId);
    Task<Response<AddRoomRequestDto>> AddRoomAsync(AddRoomRequestDto roomDto);
    Task<Response<UpdateRoomRequestDto>> UpdateRoomAsync(UpdateRoomRequestDto roomDto, Guid roomId);
    Task<Response<bool>> DeleteRoomAsync(Guid roomId);
    Task<Response<List<ListRoomResponseDto>>> GetFilteredRooms(decimal? minPrice, decimal? maxPrice, string? location);
}