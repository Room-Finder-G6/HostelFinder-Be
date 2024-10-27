using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class RoomRepository : BaseGenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> RoomExistsAsync(string roomName, Guid hostelId)
        {
            return await _dbContext.Rooms.AnyAsync(r => r.RoomName == roomName && r.HostelId == hostelId);
        }
    }
}
