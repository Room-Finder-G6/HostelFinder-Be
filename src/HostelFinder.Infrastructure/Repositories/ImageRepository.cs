using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class ImageRepository : BaseGenericRepository<Image>, IImageRepository
    {
        public ImageRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Image> GetImageUrlByRoomId(Guid roomId)
        {
            var imageUrl = await _dbContext.Images.FirstOrDefaultAsync(x => x.RoomId == roomId);
            if (imageUrl == null)
            {
                return null;
            }
            return imageUrl;
        }
    }
}
