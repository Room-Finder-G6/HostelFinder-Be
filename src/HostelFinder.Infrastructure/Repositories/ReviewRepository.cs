using DocumentFormat.OpenXml.InkML;
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
            var ratings = await _dbContext.Reviews
                .Where(r => r.HostelId == hostelId)
                .Select(r => r.rating)
                .ToListAsync();  // Executes the query and retrieves the data into memory

            // Now calculate the average on the client side
            if (ratings.Any())  // Check if there are any ratings
            {
                return (float)ratings.Average();  // Calculate the average in memory
            }

            return 0;  // Return 0 if there are no ratings
        }
    }
}
