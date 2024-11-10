using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IInVoiceRepository : IBaseGenericRepository<Invoice>
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
