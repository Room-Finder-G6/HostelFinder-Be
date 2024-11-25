using DocumentFormat.OpenXml.Drawing.Charts;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{

    public class RentalContractRepository : BaseGenericRepository<RentalContract>, IRentalContractRepository
    {
        public RentalContractRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<RentalContract?> CheckExpiredContractAsync(Guid roomId, DateTime startDate, DateTime? endDate)
        {
            var roomExists = await _dbContext.RentalContracts
                        .AnyAsync(rt => rt.RoomId == roomId);

            if (!roomExists)
            {
                return null;
            }
            var rentalContract = await _dbContext.RentalContracts
            .FirstOrDefaultAsync(rt => rt.RoomId == roomId
                                        && rt.EndDate.HasValue
                                        && (rt.StartDate <= endDate) && (rt.EndDate >= startDate)
                                        );
            return rentalContract;
        }


        public async Task<RentalContract?> GetRoomRentalContrctByRoom(Guid roomId)
        {
            var currentDate = DateTime.Now.Date;
            return await _dbContext.RentalContracts
                .Include(rt => rt.Room)
                .Include(rt => rt.Tenant)
                .Where(rt => rt.RoomId == roomId
                        && rt.StartDate.Date <= currentDate
                            && (rt.EndDate == null || rt.EndDate >= currentDate))
                .FirstOrDefaultAsync();
        }
    }
}
