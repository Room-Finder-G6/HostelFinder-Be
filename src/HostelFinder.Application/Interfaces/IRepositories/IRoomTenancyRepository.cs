using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IRoomTenancyRepository : IBaseGenericRepository<RoomTenancy>
    {
        Task<int> CountCurrentTenantsAsync(Guid roomId);
    }
}
