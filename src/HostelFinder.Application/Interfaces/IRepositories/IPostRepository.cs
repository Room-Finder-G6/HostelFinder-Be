using HostelFinder.Application.Common;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IPostRepository : IBaseGenericRepository<Post>
{
    Task<Post?> GetAllRoomFeaturesByRoomId(Guid roomId);
    Task<IEnumerable<Post>> GetFilteredRooms(decimal? minPrice, decimal? maxPrice, string? location, RoomType? roomType);
    Task<RoomAmenities> AddRoomAmenitiesAsync(RoomAmenities roomAmenities);
    Task<List<Post>> GetPostsByUserIdAsync(Guid userId);
    Task<(IEnumerable<Post> Data, int TotalRecords)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
}