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
        var room = await _dbContext.Rooms.Where(x => x.Id == roomId)
            .Include(x => x.RoomDetails)
            .Include(x=>x.RoomAmenities)
            .Include(x => x.RoomType)
            .Include(x => x.ServiceCosts)
            .FirstOrDefaultAsync(x => x.Id == roomId);
        return room;
    }

    /*public async Task AddRoom(Room room)
    {
        _dbContext.Entry(room.RoomDetails).State = EntityState.Added;
        _dbContext.Entry(room.RoomAmenities).State = EntityState.Added;
        await _dbContext.Rooms.AddAsync(room);
    }*/
}