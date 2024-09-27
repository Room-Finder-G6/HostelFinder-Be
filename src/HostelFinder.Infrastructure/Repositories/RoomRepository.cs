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


    public Task<IQueryable<Room>> GetAllRoomFeaturesByRoomId(Guid roomId)
    {
        var room = _dbContext.Rooms.Where(x => x.Id == roomId)
            .Include(x => x.RoomDetails)
            .Include(x=>x.RoomAmenities)
            .Include(x => x.RoomType)
            .Include(x => x.ServiceCosts);
        return Task.FromResult<IQueryable<Room>>(room);
    }
}