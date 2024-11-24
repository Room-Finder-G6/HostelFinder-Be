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
            var rentalContract = await _dbContext.RentalContracts
                .FirstOrDefaultAsync(rt => rt.RoomId == roomId && rt.EndDate.HasValue 
                                                && (startDate >= rt.StartDate && startDate <= rt.EndDate)
                                                || (endDate >= rt.StartDate && endDate <= rt.EndDate));
            return rentalContract;
        }


        public async Task<RentalContract?> GetRoomRentalContrctByRoom(Guid roomId)
        {
            return await _dbContext.RentalContracts
                .Include(rt => rt.Room)
                .Include(rt => rt.Tenant)
                .OrderByDescending(rt => rt.EndDate)
                .FirstOrDefaultAsync(x => x.RoomId == roomId);
        }
    }
}
