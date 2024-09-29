using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
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
            .Include(x => x.ServiceCosts)
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == roomId);
        return room;
    }
}