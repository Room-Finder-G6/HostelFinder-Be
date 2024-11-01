using HostelFinder.Application.Common;
using HostelFinder.Application.DTOs.Post.Responses;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IPostRepository : IBaseGenericRepository<Post>
{
    Task<IEnumerable<Post>> GetFilteredPosts(decimal? minPrice, decimal? maxPrice, string? location, RoomType? roomType);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId);
    Task<(IEnumerable<Post> Data, int TotalRecords)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
    Task<Post?> GetPostByIdAsync(Guid postId);
    Task<Post?> GetPostByIdWithHostelAsync(Guid postId);
}