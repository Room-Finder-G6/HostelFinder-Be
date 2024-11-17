using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;

namespace HostelFinder.Infrastructure.Repositories
{
    public class TenantRepository : BaseGenericRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
