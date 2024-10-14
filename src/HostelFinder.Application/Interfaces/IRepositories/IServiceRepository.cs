using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IServiceRepository : IBaseGenericRepository<Service>
    {
        Task<bool> CheckDuplicateServiceAsync(string serviceName);
    }
}