using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Exceptions;
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
                .Where(rt => rt.RoomId == roomId && (rt.MoveOutDate == null || rt.MoveOutDate > DateTime.Now) && !rt.IsDeleted)
                .CountAsync();
        }
        // lấy ra danh sách người đang thuê trọ hiện tại
        public async Task<List<RoomTenancy>> GetRoomTenacyByIdAsync(Guid roomId)
        {
            return await _dbContext.RoomTenancies
                .Include(x => x.Tenant)
                .Include(x => x.Room)
                .Where(x => x.RoomId == roomId && x.MoveInDate <=DateTime.Now
                                    &&(x.MoveOutDate == null || x.MoveOutDate > DateTime.Now) && !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
            
        }

        public async Task<List<RoomTenancy>> GetTenacyCurrentlyByRoom(Guid roomId, DateTime startDate, DateTime? endDate)
        {
            var tanecyCurrentInRoomList =  await _dbContext.RoomTenancies
               .Include(x => x.Tenant)
               .Include(x => x.Room)
               .Where(x => x.RoomId == roomId 
                                    && endDate.HasValue 
                                        && x.MoveOutDate.HasValue 
                                            && startDate <= x.MoveInDate && x.MoveInDate <= endDate
                                                && !x.IsDeleted)
               .ToListAsync();
            if(tanecyCurrentInRoomList.Count == 0)
            {
                return null;
            }
            return tanecyCurrentInRoomList;
        }
    }
}
