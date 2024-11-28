using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IUserMembershipRepository : IBaseGenericRepository<UserMembership>
    {
        Task<UserMembership> GetByUserIdAsync(Guid userId);
        Task<List<UserMembership>> GetExpiredMembershipsAsync();
    }
}

