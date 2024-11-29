using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Exceptions;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class TenantRepository : BaseGenericRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Tenant> GetByIdentityCardNumber(string identityCardNumber)
        {
            var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(x => x.IdentityCardNumber == identityCardNumber && !x.IsDeleted);
            return tenant;
        }
    }
}
