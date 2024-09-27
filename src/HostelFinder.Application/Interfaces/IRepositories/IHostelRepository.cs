using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IHostelRepository : IBaseGenericRepository<Hostel>
{
    Task<IEnumerable<Hostel>> GetHostelsByLandlordIdAsync(Guid landlordId);
}
