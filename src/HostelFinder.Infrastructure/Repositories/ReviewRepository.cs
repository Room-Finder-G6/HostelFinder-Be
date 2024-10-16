using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class ReviewRepository : BaseGenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Review>> GetReviewsByHostelIdAsync(Guid hostelId)
        {
            return await _dbContext.Reviews
                .Where(r => r.HostelId == hostelId)
                .ToListAsync();
        }
        public async Task<float> GetAverageRatingForHostelAsync(Guid hostelId)
        {
            return (float)await _dbContext.Reviews
                .Where(r => r.HostelId == hostelId)
                .AverageAsync(r => r.rating);
        }
    }
}
