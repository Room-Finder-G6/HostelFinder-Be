using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HostelFinder.Infrastructure.Repositories
{
    public class ServiceCostRepository : BaseGenericRepository<ServiceCost>, IServiceCostRepository
    {
        public ServiceCostRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ServiceCost> CheckExistingServiceCostAsync(Guid hostelId, Guid serviceId, DateTime effectiveFrom)
        {
            return await _dbContext.ServiceCosts.SingleOrDefaultAsync(sc => sc.HostelId == hostelId &&
                                                                        sc.ServiceId == serviceId
                                                                            && sc.EffectiveFrom <= effectiveFrom
                                                                                && (sc.EffectiveTo == null || sc.EffectiveTo >= effectiveFrom));
        }

        public async Task<List<ServiceCost>> GetAllServiceCostListAsync()
        {
            return await _dbContext.ServiceCosts.Include(sr => sr.Hostel)
                .ThenInclude(h => h.Rooms)
                .Include(sr => sr.Service)
                .ToListAsync();
        }

        public async Task<List<ServiceCost>> GetAllServiceCostListWithConditionAsync(Expression<Func<ServiceCost, bool>> filter)
        {
            return await _dbContext.ServiceCosts.Include(sr => sr.Hostel)
            .ThenInclude(h => h.Rooms)
            .Include(sr => sr.Service)
            .Where(filter)
            .ToListAsync();
        }
    }
}
