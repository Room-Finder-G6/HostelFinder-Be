using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IServiceCostRepository : IBaseGenericRepository<ServiceCost>
    {
        Task<bool> CheckDuplicateServiceCostAsync(Guid postId, string serviceName, Guid? excludeId = null);
        Task<IEnumerable<ServiceCost>> GetServiceCostsByPostIdAsync(Guid postId);
    }
}
