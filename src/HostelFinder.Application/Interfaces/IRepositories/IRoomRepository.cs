using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IRoomRepository : IBaseGenericRepository<Room>
{
    Task<IQueryable<Room>> GetRoomsByHostelId(Guid hostelId);
    Task<IQueryable<Room>> GetRoomsByHostelId(Guid hostelId, string? search);
    Task<IQueryable<Room>> GetRoomByRoomId(Guid roomId);
    Task<IQueryable<Room>> GetRoomsByRoomType(string roomType);
    Task<IQueryable<Room>> GetRoomsByPrice(decimal minPrice, decimal maxPrice);
    Task<IQueryable<Room>> GetRoomsByTitle(string title);
    Task<Room> AddRoomAsync(Room room);
    Task<Room> UpdateRoomAsync(Room room);
    Task<Room> DeleteRoomAsync(Guid roomId);
}