using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;

namespace HostelFinder.Infrastructure.Repositories
{
    public class ImageRepository : BaseGenericRepository<Image>, IImageRepository
    {
        public ImageRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

    }
}
