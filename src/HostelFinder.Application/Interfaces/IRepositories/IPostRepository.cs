using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IPostRepository : IBaseGenericRepository<Post>
{
    Task<Post?> GetAllRoomFeaturesByRoomId(Guid roomId);
    Task<IEnumerable<Post>> GetFilteredRooms(decimal? minPrice, decimal? maxPrice, string? location, RoomType? roomType);
    Task<RoomAmenities> AddRoomAmenitiesAsync(RoomAmenities roomAmenities);
}