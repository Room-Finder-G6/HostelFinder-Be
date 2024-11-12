using HostelFinder.Application.Common;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IPostRepository : IBaseGenericRepository<Post>
{
    Task<IEnumerable<Post>> GetFilteredPosts(decimal? minPrice, decimal? maxPrice, string? location, RoomType? roomType);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId);
    Task<(IEnumerable<Post> Data, int TotalRecords)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
    Task<Post?> GetPostByIdAsync(Guid postId);
    Task<Post?> GetPostByIdWithHostelAsync(Guid postId);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<List<Post>> FilterPostsAsync(string? province, string? district, string? commune, float? size, RoomType? roomType);
}