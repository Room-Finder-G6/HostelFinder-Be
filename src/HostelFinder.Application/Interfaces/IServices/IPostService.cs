using HostelFinder.Application.DTOs.Post.Requests;
using HostelFinder.Application.DTOs.Post.Responses;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IPostService
{
    Task<Response<AddPostRequestDto>> AddPostAsync(AddPostRequestDto request, List<string> imageUrls, Guid userId);
    Task<Response<List<ListPostsResponseDto>>> GetPostsByUserIdAsync(Guid userId);
    Task<Response<bool>> DeletePostAsync(Guid postId, Guid userId);
    Task<PagedResponse<List<ListPostsResponseDto>>> GetAllPostAysnc(GetAllPostsQuery request);
    Task<Response<PostResponseDto>> GetPostByIdAsync(Guid postId);
    Task<Response<UpdatePostRequestDto>> UpdatePostAsync(Guid postId, UpdatePostRequestDto request, List<string> imageUrls);
    Task<Response<List<ListPostsResponseDto>>> FilterPostsAsync(FilterPostsRequestDto filter);
    Task<Response<PostResponseDto>> PushPostOnTopAsync(Guid postId, DateTime newDate, Guid userId);
    Task<Response<List<ListPostsResponseDto>>> GetPostsOrderedByPriorityAsync();
    Task <Response<List<ListPostsResponseDto>>> GetAllPostWithPriceAndStatusAndTime();
    Task<PagedResponse<List<ListPostsResponseDto>>> GetPostsOrderedByPriorityAsync(int pageIndex, int pageSize);
    Task<PagedResponse<List<ListPostsResponseDto>>> GetAllPostWithPriceAndStatusAndTime(int pageIndex, int pageSize);
}