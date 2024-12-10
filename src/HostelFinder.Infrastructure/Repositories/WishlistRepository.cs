using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class WishlistRepository : BaseGenericRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Wishlist> GetWishlistByUserIdAsync(Guid userId)
        {
            return await _dbContext.Wishlists
                .Include(w => w.WishlistPosts)
                    .ThenInclude(wr => wr.Post)
                        .ThenInclude(p => p.Hostel) 
                        .ThenInclude(h => h.Address) 
                .Include(w => w.WishlistPosts)
                    .ThenInclude(wr => wr.Post)
                        .ThenInclude(p => p.Room)
                .Include(w => w.WishlistPosts)
                    .ThenInclude(wr => wr.Post)
                        .ThenInclude(p => p.MembershipServices) 
                            .ThenInclude(ms => ms.Membership)
                .Include(w => w.WishlistPosts)
                        .ThenInclude(wr => wr.Post)
                            .ThenInclude(p => p.Images)
                .OrderByDescending(o => o.CreatedOn)
                .FirstOrDefaultAsync(w => w.UserId == userId && !w.IsDeleted);
        }


        public async Task AddRoomToWishlistAsync(WishlistPost wishlistRoom)
        {
            await _dbContext.WishlistPosts.AddAsync(wishlistRoom);
            await _dbContext.SaveChangesAsync();
        }

    }
}
