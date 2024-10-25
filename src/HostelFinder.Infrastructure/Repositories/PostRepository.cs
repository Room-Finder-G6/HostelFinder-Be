using DocumentFormat.OpenXml.InkML;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HostelFinder.Infrastructure.Repositories;

public class PostRepository : BaseGenericRepository<Post>, IPostRepository
{
    public PostRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Post?> GetAllRoomFeaturesByRoomId(Guid roomId)
    {
        var room = await _dbContext.Posts
            .Include(x => x.Room)
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == roomId);
        return room;
    }

    public async Task<IEnumerable<Post>> GetFilteredRooms(decimal? minPrice, decimal? maxPrice, string? location, RoomType? roomType)
    {
        var query = _dbContext.Posts.AsQueryable();

        if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
        {
            throw new ArgumentException("Minimum price cannot be greater than maximum price");
        }

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(x => x.Hostel.Address.Province.Contains(location));
        }

        return await query.ToListAsync();
    }

    public async Task<RoomAmenities> AddRoomAmenitiesAsync(RoomAmenities roomAmenity)
    {
        await _dbContext.RoomAmenities.AddAsync(roomAmenity);
        await _dbContext.SaveChangesAsync();
        return roomAmenity;
    }

    public async Task<(IEnumerable<Post> Data, int TotalRecords)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();

        var baseQuery = _dbContext
            .Posts
            .Where(p => searchPhraseLower == null || (p.Title.ToLower().Contains(searchPhraseLower)
                                                || p.Description.ToLower().Contains(searchPhraseLower)));

        var totalCount = await baseQuery.CountAsync();

        if (sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Post, object>>>
            {
                {nameof(Post.Title), r => r.Title },
                {nameof(Post.Description), r => r.Description },
            };

            var selectedColumn = columnsSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var posts = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (Data : posts,TotalRecords : totalCount);
    }

    public async Task<List<Post>> GetPostsByUserIdAsync(Guid userId)
    {
        return await _dbContext.Posts
            .Where(p => p.Id == userId)
            .ToListAsync();
    }

}