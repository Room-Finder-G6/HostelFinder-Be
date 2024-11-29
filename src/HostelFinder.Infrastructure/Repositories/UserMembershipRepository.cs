using DocumentFormat.OpenXml.InkML;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class UserMembershipRepository : BaseGenericRepository<UserMembership>, IUserMembershipRepository
    {
        public UserMembershipRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserMembership> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.UserMemberships
                .Include(um => um.Membership)
                .ThenInclude(m => m.MembershipServices)
                .FirstOrDefaultAsync(um => um.UserId == userId && !um.IsDeleted);
        }

        public async Task<List<UserMembership>> GetExpiredMembershipsAsync()
        {
            return await _dbContext.UserMemberships
                 .Where(um => um.ExpiryDate < DateTime.Now && !um.IsDeleted)
                 .ToListAsync();
        }

    }

}
