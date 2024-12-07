using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;

namespace HostelFinder.Infrastructure.Repositories
{
    public class AddressStoryRepository : BaseGenericRepository<AddressStory>, IAddressStoryRepository
    {
        public AddressStoryRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
