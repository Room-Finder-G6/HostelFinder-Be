using HostelFinder.Application.DTOs.Post.Requests;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IPostService
{
    Task<Response<AddPostRequestDto>> AddPostAsync(AddPostRequestDto request);
    /*Task<LandlordResponseDto> GetLandlordByPostIdAsync(Guid hostelId);
    Task<HostelResponseDto> GetHostelByPostIdAsync(Guid postId);
    Task<PagedResponse<List<ListPostResponseDto>>> GetAllPostAysnc(GetAllPostsQuery request);
    Task<Response<List<ListPostResponseDto>>> GetPostsByUserIdAsync(Guid userId);*/
}