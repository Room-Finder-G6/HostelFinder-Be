using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;

namespace HostelFinder.Infrastructure.Repositories
{
    public class InVoiceRepository : BaseGenericRepository<Invoice>, IInVoiceRepository
    {
        public InVoiceRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
