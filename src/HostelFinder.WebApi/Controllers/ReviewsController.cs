using HostelFinder.Application.DTOs.Review.Request;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {

        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] AddReviewRequestDto reviewDto)
        {
            var result = await _reviewService.AddReviewAsync(reviewDto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result);
        }

        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateReview(Guid reviewId, [FromBody] UpdateReviewRequestDto reviewDto)
        {
            var result = await _reviewService.UpdateReviewAsync(reviewId, reviewDto);
            if (!result.Succeeded)
            {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(Guid reviewId)
        {
            var result = await _reviewService.DeleteReviewAsync(reviewId);
            if (!result.Succeeded)
            {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }

        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetReviewById(Guid reviewId)
        {
            var result = await _reviewService.GetReviewByIdAsync(reviewId);
            if (!result.Succeeded)
            {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }

        [HttpGet("hostel/{hostelId}")]
        public async Task<IActionResult> GetReviewsForHostel(Guid hostelId)
        {
            var result = await _reviewService.GetReviewsForHostelAsync(hostelId);
            if (!result.Succeeded)
            {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }
    }
}
