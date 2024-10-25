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
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task AddRoomToWishlistAsync(WishlistPost wishlistRoom)
        {
            await _dbContext.WishlistPosts.AddAsync(wishlistRoom);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRoomFromWishlistAsync(WishlistPost wishlistRoom)
        {
            _dbContext.Set<WishlistPost>().Remove(wishlistRoom);
            await _dbContext.SaveChangesAsync();
        }
    }
}
