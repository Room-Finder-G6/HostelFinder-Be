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
            var imageUrl = await _dbContext.Images.FirstOrDefaultAsync(x => x.RoomId == roomId && !x.IsDeleted);
            if (imageUrl == null)
            {
                return null;
            }
            return imageUrl;
        }

        public async Task<List<Image>> GetImagesByHostelIdAsync(Guid hostelId)
        {
            return await _dbContext.Images
                .Where(img => img.HostelId == hostelId && !img.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Image>> GetImagesByPostIdAsync(Guid postId)
        {
            return await _dbContext.Images
                .Where(img => img.PostId == postId && !img.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<string>> GetAllUrlRoomPicture(Guid roomId)
        {
            var urlImages = await _dbContext.Images
                .Where(img => img.RoomId == roomId && !img.IsDeleted)
                .Select(img => img.Url)
                .ToListAsync();
            if(!urlImages.Any() || urlImages == null)
            {
                return new List<string>();
            }
            return urlImages;

        }
    }
}
