using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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
    }
}
