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
                .FirstOrDefaultAsync(um => um.UserId == userId);
        }
    }

}
