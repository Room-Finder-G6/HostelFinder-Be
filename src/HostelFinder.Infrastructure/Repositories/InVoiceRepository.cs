using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace HostelFinder.Infrastructure.Repositories
{
    public class InVoiceRepository : BaseGenericRepository<Invoice>, IInVoiceRepository
    {
        public InVoiceRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
    }
}
