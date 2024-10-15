using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories;

public class RoomRepository : BaseGenericRepository<Room>, IRoomRepository
{
    public RoomRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Room> GetAllRoomFeaturesByRoomId(Guid roomId)
    {
        var room = await _dbContext.Rooms
            .Include(x => x.RoomDetails)
            .Include(x => x.RoomAmenities)
            .ThenInclude(x => x.Amenity)
            .Include(x => x.ServiceCosts)
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == roomId);
        return room;
    }

    public async Task<IEnumerable<Room>> GetFilteredRooms(decimal? minPrice, decimal? maxPrice, string? location, RoomType roomType)
    {
        var rooms = await ListAllAsync();

        if (minPrice.HasValue)
        {
            rooms = rooms.Where(x => x.MonthlyRentCost >= minPrice.Value).ToList();
        }

        if(maxPrice.HasValue)
        {
            rooms = rooms.Where(x => x.MonthlyRentCost <= maxPrice.Value).ToList();
        }
        
        if (!string.IsNullOrEmpty(location))
        {
            rooms = rooms.Where(x => x.Hostel.Address.Province.Contains(location)).ToList();
        }
        
        if (roomType != RoomType.None) // Assuming RoomType.None is a default value indicating no filter
        {
            rooms = rooms.Where(x => x.RoomType == roomType).ToList();
        }
        
        return rooms;
    }

    public async Task<RoomAmenities> AddRoomAmenitiesAsync(RoomAmenities roomAmenity)
    {
        await _dbContext.RoomAmenities.AddAsync(roomAmenity);
        await _dbContext.SaveChangesAsync();
        return roomAmenity;
    }
}