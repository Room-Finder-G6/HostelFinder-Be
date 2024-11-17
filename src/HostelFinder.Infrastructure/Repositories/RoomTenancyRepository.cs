using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class RoomTenancyRepository : BaseGenericRepository<RoomTenancy>, IRoomTenancyRepository
    {
        public RoomTenancyRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> CountCurrentTenantsAsync(Guid roomId)
        {
            return await _dbContext.RoomTenancies
                .Where(rt => rt.RoomId == roomId && rt.MoveOutDate == null)
                .CountAsync();
        }
    }
}
