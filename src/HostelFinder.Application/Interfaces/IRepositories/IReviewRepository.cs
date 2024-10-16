using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IReviewRepository : IBaseGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByHostelIdAsync(Guid hostelId);
        Task<float> GetAverageRatingForHostelAsync(Guid hostelId);
    }
}
