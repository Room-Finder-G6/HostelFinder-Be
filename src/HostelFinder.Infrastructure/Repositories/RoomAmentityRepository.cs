using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;

namespace HostelFinder.Infrastructure.Repositories
{
    public class RoomAmentityRepository : BaseGenericRepository<RoomAmenities>, IRoomAmentityRepository
    {
        public RoomAmentityRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
