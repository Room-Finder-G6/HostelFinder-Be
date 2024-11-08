using HostelFinder.Application.DTOs.Post.Requests;
using HostelFinder.Application.DTOs.Post.Responses;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IPostService
{
    Task<Response<AddPostRequestDto>> AddPostAsync(AddPostRequestDto request, List<string> imageUrls, Guid userId);
    /*Task<LandlordResponseDto> GetLandlordByPostIdAsync(Guid hostelId);
    Task<HostelResponseDto> GetHostelByPostIdAsync(Guid postId);*/
    Task<Response<List<ListPostsResponseDto>>> GetPostsByUserIdAsync(Guid userId);
    Task<Response<bool>> DeletePostAsync(Guid postId, Guid userId);
    Task<PagedResponse<List<ListPostsResponseDto>>> GetAllPostAysnc(GetAllPostsQuery request);
    Task<Response<PostResponseDto>> GetPostByIdAsync(Guid postId);
}