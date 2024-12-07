using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IStoryRepository : IBaseGenericRepository<Story>
    {
        Task<Story> GetStoryByIdAsync(Guid id);
        Task<IEnumerable<Story>> GetAllStories();
        Task<IEnumerable<Story>> GetAllStoriesNoCondition();
        Task<IEnumerable<Story>> GetStoriesByUserId(Guid userId);
    }
}
