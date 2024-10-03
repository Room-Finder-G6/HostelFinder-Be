using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

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
            .Include(x => x.ServiceCosts)
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == roomId);
        return room;
    }

    public async Task<IEnumerable<Room>> GetFilteredRooms(decimal? minPrice, decimal? maxPrice, string? location)
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
        
        return rooms;
    }
}