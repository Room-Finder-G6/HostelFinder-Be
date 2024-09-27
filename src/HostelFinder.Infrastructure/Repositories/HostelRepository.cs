using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories;

public class HostelRepository : BaseGenericRepository<Hostel>, IHostelRepository
{
    public HostelRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<IEnumerable<Hostel>> GetHostelsByLandlordIdAsync(Guid landlordId)
    {
        return await _dbContext.Hostels.Where(h => h.LandlordId == landlordId).ToListAsync();
    }
}
