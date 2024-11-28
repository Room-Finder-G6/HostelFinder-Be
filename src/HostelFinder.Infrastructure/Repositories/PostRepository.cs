using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using HostelFinder.Application.Wrappers;
using HostelFinder.Application.Helpers;

namespace HostelFinder.Infrastructure.Repositories;

public class PostRepository : BaseGenericRepository<Post>, IPostRepository
{
    public PostRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(IEnumerable<Post> Data, int TotalRecords)> GetAllMatchingAsync(string? searchPhrase,
        int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
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
                { nameof(Post.Title), r => r.Title },
                { nameof(Post.Description), r => r.Description },
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

        return (Data: posts, TotalRecords: totalCount);
    }

    public Task<Post?> GetPostByIdAsync(Guid postId)
    {
        return _dbContext.Posts
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == postId);
    }

    public Task<Post?> GetPostByIdWithHostelAsync(Guid postId)
    {
        return _dbContext.Posts.Include(p => p.Hostel).FirstOrDefaultAsync(x => x.Id == postId);
    }

    public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId)
    {
        var posts = await _dbContext.Posts
            .Include(x => x.Hostel)
            .ThenInclude(h => h.Address)
            .Include(x => x.Room)
            .Include(x => x.Images)
            .Where(x => x.Hostel.LandlordId == userId)
            .AsNoTracking() // Tăng hiệu suất cho truy vấn chỉ đọc
            .ToListAsync();
        return posts;
    }


    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task<List<Post>> FilterPostsAsync(string? province, string? district, string? commune,
        decimal? minSize, decimal? maxSize, decimal? minPrice, decimal? maxPrice, RoomType? roomType)
    {
        var query = _dbContext.Posts
            .Include(p => p.Hostel)
            .ThenInclude(h => h.Address)
            .Include(p => p.Room)
            .Include(p => p.Images)
            .Include(p => p.MembershipServices)
            .ThenInclude(p => p.Membership)
            .AsQueryable();

        if (!string.IsNullOrEmpty(province))
            query = query.Where(p => p.Hostel.Address.Province == province);

        if (!string.IsNullOrEmpty(district))
            query = query.Where(p => p.Hostel.Address.District == district);

        if (!string.IsNullOrEmpty(commune))
            query = query.Where(p => p.Hostel.Address.Commune == commune);

        if (minSize.HasValue)
            query = query.Where(p => p.Room.Size >= minSize.Value);

        if (maxSize.HasValue)
            query = query.Where(p => p.Room.Size <= maxSize.Value);

        if (roomType.HasValue)
            query = query.Where(p => p.Room.RoomType == roomType);

        if (minPrice.HasValue)
            query = query.Where(p => p.Room.MonthlyRentCost >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Room.MonthlyRentCost <= maxPrice.Value);

        return await query.ToListAsync();
    }

    public async Task<List<Post>> GetPostsOrderedByMembershipPriceAndCreatedOnAsync()
    {
        return await _dbContext.Posts
            .Include(x => x.Hostel)
            .ThenInclude(x => x.Address)
            .Include(x => x.Images)
            .Include(p => p.MembershipServices)
            .ThenInclude(ms => ms.Membership)
            .OrderByDescending(p => p.MembershipServices.Membership.Price)
            .ThenByDescending(p => p.CreatedOn)
            .ToListAsync();
    }

    public async Task<PagedResponse<List<Post>>> GetPostsOrderedByMembershipPriceAndCreatedOnAsync(int pageIndex, int pageSize)
    {
        var query = _dbContext.Posts
            .Include(x => x.Hostel)
            .ThenInclude(x => x.Address)
            .Include(x => x.Images)
            .Include(p => p.MembershipServices)
            .ThenInclude(ms => ms.Membership)
            .OrderByDescending(p => p.MembershipServices.Membership.Price)
            .ThenByDescending(p => p.CreatedOn)
            .AsNoTracking();

        var totalRecords = await query.CountAsync();
        var posts = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return PaginationHelper.CreatePagedResponse(posts, pageIndex, pageSize, totalRecords);
    }

    public async Task<List<Post>> GetAllPostsOrderedAsync()
    {
        return await _dbContext.Posts
            .Include(p => p.Hostel)
            .ThenInclude(h => h.Address)
            .Include(p => p.Images)
            .Include(p => p.MembershipServices)
            .ThenInclude(ms => ms.Membership)
            .OrderByDescending(p => p.MembershipServices.Membership.Price)
            .ThenByDescending(p => p.Status) 
            .ThenByDescending(p => p.CreatedOn) 
            .ToListAsync();
    }

    public async Task<PagedResponse<List<Post>>> GetAllPostsOrderedAsync(int pageIndex, int pageSize)
    {
        var query = _dbContext.Posts
            .Include(p => p.Hostel)
            .ThenInclude(h => h.Address)
            .Include(p => p.Images)
            .Include(p => p.MembershipServices)
            .ThenInclude(ms => ms.Membership)
            .OrderByDescending(p => p.MembershipServices.Membership.Price)
            .ThenByDescending(p => p.Status)
            .ThenByDescending(p => p.CreatedOn)
            .AsNoTracking();

        var totalRecords = await query.CountAsync();
        var posts = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return PaginationHelper.CreatePagedResponse(posts, pageIndex, pageSize, totalRecords);
    }

}