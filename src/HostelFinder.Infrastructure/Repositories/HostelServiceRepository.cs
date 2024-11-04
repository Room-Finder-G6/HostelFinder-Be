using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;

namespace HostelFinder.Infrastructure.Repositories
{
    public class HostelServiceRepository : BaseGenericRepository<HostelServices>, IHostelServiceRepository
    {
        public HostelServiceRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
