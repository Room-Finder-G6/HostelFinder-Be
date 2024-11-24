using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Invoice?> GetLastInvoiceByIdAsync(Guid roomId)
        {
            return await _dbContext.InVoices
                .Include(x => x.InvoiceDetails)
                .ThenInclude(details => details.Service)
                .Where(x => x.RoomId == roomId)
                .OrderByDescending(x => x.BillingYear)
                .ThenByDescending(x => x.BillingMonth)
                .FirstOrDefaultAsync();
        }
    }
}
