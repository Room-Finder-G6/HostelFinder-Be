using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories;

public class PostRepository : BaseGenericRepository<Post>, IPostRepository
{
    public PostRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Post?> GetAllRoomFeaturesByRoomId(Guid roomId)
    {
        var room = await _dbContext.Posts
            .Include(x => x.RoomDetails)
            .Include(x => x.RoomAmenities)
            .ThenInclude(x => x.Amenity)
            .Include(x => x.ServiceCosts)
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == roomId);
        return room;
    }
    
    public async Task<IEnumerable<Post>> GetFilteredRooms(decimal? minPrice, decimal? maxPrice, string? location, RoomType? roomType)
{
    var query = _dbContext.Posts.AsQueryable();

    if (minPrice.HasValue)
    {
        query = query.Where(x => x.MonthlyRentCost >= minPrice.Value);
    }

    if (maxPrice.HasValue)
    {
        query = query.Where(x => x.MonthlyRentCost <= maxPrice.Value);
    }

    if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
    {
        throw new ArgumentException("Minimum price cannot be greater than maximum price");
    }

    if (!string.IsNullOrEmpty(location))
    {
        query = query.Where(x => x.Hostel.Address.Province.Contains(location));
    }

    if (roomType.HasValue)
    {
        query = query.Where(x => x.RoomType == roomType.Value);
    }

    return await query.ToListAsync();
}

    public async Task<RoomAmenities> AddRoomAmenitiesAsync(RoomAmenities roomAmenity)
    {
        await _dbContext.RoomAmenities.AddAsync(roomAmenity);
        await _dbContext.SaveChangesAsync();
        return roomAmenity;
    }
}