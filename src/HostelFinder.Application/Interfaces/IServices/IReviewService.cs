using HostelFinder.Application.DTOs.Review.Request;
using HostelFinder.Application.DTOs.Review.Response;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IReviewService
    {
        Task<Response<ReviewResponseDto>> AddReviewAsync(AddReviewRequestDto reviewDto);
        Task<Response<ReviewResponseDto>> UpdateReviewAsync(Guid reviewId, UpdateReviewRequestDto reviewDto);
        Task<Response<bool>> DeleteReviewAsync(Guid reviewId);
        Task<Response<ReviewResponseDto>> GetReviewByIdAsync(Guid reviewId);
        Task<IEnumerable<ReviewResponseDto>> GetReviewsForHostelAsync(Guid hostelId);
    }
}
