using AutoMapper;
using HostelFinder.Application.DTOs.Review.Request;
using HostelFinder.Application.DTOs.Review.Response;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<Response<ReviewResponseDto>> AddReviewAsync(AddReviewRequestDto reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            review.ReviewDate = DateTime.Now;
            review.CreatedBy = "System";
            review.CreatedOn = DateTime.Now;

            try
            {
                await _reviewRepository.AddAsync(review);
                var responseDto = _mapper.Map<ReviewResponseDto>(review);
                return new Response<ReviewResponseDto>(responseDto, "Thêm đánh giá thành công");
            }
            catch (Exception ex)
            {
                return new Response<ReviewResponseDto>(message: ex.Message);
            }
        }

        public async Task<Response<ReviewResponseDto>> UpdateReviewAsync(Guid reviewId, UpdateReviewRequestDto reviewDto)
        {
            var existingReview = await _reviewRepository.GetByIdAsync(reviewId);
            if (existingReview == null)
            {
                return new Response<ReviewResponseDto>("Review not found.");
            }

            _mapper.Map(reviewDto, existingReview);
            existingReview.ReviewDate = DateTime.Now;
            existingReview.LastModifiedBy = "System";
            existingReview.LastModifiedOn = DateTime.Now;

            try
            {
                await _reviewRepository.UpdateAsync(existingReview);
                var updatedReviewDto = _mapper.Map<ReviewResponseDto>(existingReview);
                return new Response<ReviewResponseDto>(updatedReviewDto, "Đánh giá cập nhật thành công!");
            }
            catch (Exception ex)
            {
                return new Response<ReviewResponseDto>(message: ex.Message);
            }
        }

        public async Task<Response<bool>> DeleteReviewAsync(Guid reviewId)
        {
            var existingReview = await _reviewRepository.GetByIdAsync(reviewId);
            if (existingReview == null)
            {
                return new Response<bool>(false, "Review not found.");
            }

            try
            {
                await _reviewRepository.DeleteAsync(reviewId);
                return new Response<bool>(true, "Xóa đánh giá thành công!");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message);
            }
        }

        public async Task<Response<ReviewResponseDto>> GetReviewByIdAsync(Guid reviewId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null)
            {
                return new Response<ReviewResponseDto>("Review not found.");
            }

            var reviewResponseDto = _mapper.Map<ReviewResponseDto>(review);
            return new Response<ReviewResponseDto>(reviewResponseDto);
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsForHostelAsync(Guid hostelId)
        {
            var reviews = await _reviewRepository.GetReviewsByHostelIdAsync(hostelId);
            return _mapper.Map<IEnumerable<ReviewResponseDto>>(reviews);
        }

    }
}
