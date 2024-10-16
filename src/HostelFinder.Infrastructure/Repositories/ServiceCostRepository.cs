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

        public async Task<bool> CheckDuplicateServiceCostAsync(Guid postId, string serviceName, Guid? excludeId = null)
        {
            var query = _dbContext.ServiceCosts.Where(sc =>
                sc.PostId == postId &&
                sc.ServiceName == serviceName);

            if (excludeId.HasValue)
            {
                query = query.Where(sc => sc.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<ServiceCost>> GetServiceCostsByPostIdAsync(Guid postId)
        {
            return await _dbContext.ServiceCosts.Where(sc => sc.PostId == postId).ToListAsync();
        }
    }
}
