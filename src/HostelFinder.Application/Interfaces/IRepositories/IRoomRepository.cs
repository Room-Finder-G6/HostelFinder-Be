using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IRoomRepository : IBaseGenericRepository<Room>
{
    Task<Room> GetAllRoomFeaturesByRoomId(Guid roomId);
    Task<IEnumerable<Room>> GetFilteredRooms(decimal? minPrice, decimal? maxPrice, string? location, RoomType roomType);
    Task<RoomAmenities> AddRoomAmenitiesAsync(RoomAmenities roomAmenities);
}