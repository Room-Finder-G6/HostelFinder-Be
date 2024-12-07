using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class StoryRepository : BaseGenericRepository<Story>, IStoryRepository
    {
        public StoryRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Story> GetStoryByIdAsync(Guid id)
        {
            return await _dbContext.Stories
                .AsNoTracking()
                .Include(s => s.Images)  
                .Include(s => s.AddressStory)  
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Story>> GetAllStories()
        {
            return await _dbContext.Stories
                .AsNoTracking()
                .Where(s => s.BookingStatus == BookingStatus.Accepted && !s.IsDeleted)
                .Include(s => s.Images)  
                .Include(s => s.AddressStory)
                .Include(s => s.User)
                .ToListAsync();  
        }

        public async Task<IEnumerable<Story>> GetAllStoriesNoCondition()
        {
            return await _dbContext.Stories
                .AsNoTracking()
                .Include(s => s.Images)
                .Include(s => s.AddressStory)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Story>> GetStoriesByUserId(Guid userId)
        {
            return await _dbContext.Stories
                .AsNoTracking()
                .Where(s => s.UserId == userId && !s.IsDeleted)
                .Include(s => s.Images)
                .Include(s => s.AddressStory)
                .Include(s => s.User)
                .ToListAsync();
        }
    }
}
