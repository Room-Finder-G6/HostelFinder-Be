using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class ServiceRepository : BaseGenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _dbContext.Services.ToListAsync();
        }

        public async Task<Service?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Services.FindAsync(id);
        }

    }
}
