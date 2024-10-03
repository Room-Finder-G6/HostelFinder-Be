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
                .Include(w => w.WishlistRooms)
                .ThenInclude(wr => wr.Room)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task AddRoomToWishlistAsync(WishlistRoom wishlistRoom)
        {
            await _dbContext.WishlistRooms.AddAsync(wishlistRoom);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRoomFromWishlistAsync(WishlistRoom wishlistRoom)
        {
            _dbContext.Set<WishlistRoom>().Remove(wishlistRoom);
            await _dbContext.SaveChangesAsync();
        }
    }
}
