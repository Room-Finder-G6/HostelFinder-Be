using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.DTOs.Post.Requests;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.Users.Response;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IPostService
{
    Task<Response<AddPostRequestDto>> AddPostAsync(AddPostRequestDto request, List<string> imageUrls);
    /*Task<LandlordResponseDto> GetLandlordByPostIdAsync(Guid hostelId);
    Task<HostelResponseDto> GetHostelByPostIdAsync(Guid postId);
    Task<PagedResponse<List<ListPostResponseDto>>> GetAllPostAysnc(GetAllPostsQuery request);
    Task<Response<List<ListPostResponseDto>>> GetPostsByUserIdAsync(Guid userId);*/
}