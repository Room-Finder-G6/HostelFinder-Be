using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IUserMembershipRepository : IBaseGenericRepository<UserMembership>
    {
        Task<UserMembership> GetUserMembershipByUserIdAsync(Guid userId);
        Task<List<UserMembership>> GetByUserIdAsync(Guid userId);
        Task<List<UserMembership>> GetExpiredMembershipsAsync();
        Task<UserMembership> GetTrialMembershipByUserIdAsync(Guid userId);
        Task<UserMembership> GetByUserIdAndMembershipIdAsync(Guid userId, Guid membershipId);
    }
}

