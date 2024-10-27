using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;

namespace HostelFinder.Infrastructure.Repositories
{
    public class RoomRepository : BaseGenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
