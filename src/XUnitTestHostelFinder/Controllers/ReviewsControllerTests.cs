using HostelFinder.Application.DTOs.Review.Request;
using HostelFinder.Application.DTOs.Review.Response;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HostelFinder.UnitTests.Controllers
{
    public class ReviewsControllerTests
    {
        private readonly ReviewsController _controller;
        private readonly Mock<IReviewService> _reviewServiceMock;

        public ReviewsControllerTests()
        {
            _reviewServiceMock = new Mock<IReviewService>();
            _controller = new ReviewsController(_reviewServiceMock.Object);
        }

        [Fact]
        public async Task AddReview_ReturnsOkResult_WhenAdditionSucceeds()
        {
            // Arrange
            var reviewDto = new AddReviewRequestDto
            {
                HostelId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 5,
                Comment = "Great place!"
            };

            var mockResponse = new Response<ReviewResponseDto>
            {
                Data = new ReviewResponseDto { /* populate with necessary data */ },
                Succeeded = true
            };

            _reviewServiceMock
                .Setup(service => service.AddReviewAsync(reviewDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddReview(reviewDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<ReviewResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task AddReview_ReturnsBadRequest_WhenAdditionFails()
        {
            // Arrange
            var reviewDto = new AddReviewRequestDto
            {
                HostelId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 5,
                Comment = "Great place!"
            };

            var mockResponse = new Response<ReviewResponseDto>
            {
                Succeeded = false,
                Errors = new List<string> { "Addition failed" }
            };

            _reviewServiceMock
                .Setup(service => service.AddReviewAsync(reviewDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddReview(reviewDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<List<string>>(badRequestResult.Value);
            Assert.Contains("Addition failed", returnValue);
        }

        [Fact]
        public async Task UpdateReview_ReturnsOkResult_WhenUpdateSucceeds()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var updateReviewDto = new UpdateReviewRequestDto
            {
                Rating = 4,
                Comment = "Good place!"
            };

            var mockResponse = new Response<ReviewResponseDto>
            {
                Data = new ReviewResponseDto { /* populate with necessary data */ },
                Succeeded = true
            };

            _reviewServiceMock
                .Setup(service => service.UpdateReviewAsync(reviewId, updateReviewDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateReview(reviewId, updateReviewDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<ReviewResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task UpdateReview_ReturnsNotFound_WhenUpdateFails()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var updateReviewDto = new UpdateReviewRequestDto
            {
                Rating = 4,
                Comment = "Good place!"
            };

            var mockResponse = new Response<ReviewResponseDto>
            {
                Succeeded = false,
                Errors = new List<string> { "Update failed" }
            };

            _reviewServiceMock
                .Setup(service => service.UpdateReviewAsync(reviewId, updateReviewDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateReview(reviewId, updateReviewDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<List<string>>(notFoundResult.Value);
            Assert.Contains("Update failed", returnValue);
        }

        [Fact]
        public async Task DeleteReview_ReturnsOkResult_WhenDeletionSucceeds()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Data = true,
                Succeeded = true
            };

            _reviewServiceMock
                .Setup(service => service.DeleteReviewAsync(reviewId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteReview(reviewId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task DeleteReview_ReturnsNotFound_WhenDeletionFails()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Succeeded = false,
                Errors = new List<string> { "Deletion failed" }
            };

            _reviewServiceMock
                .Setup(service => service.DeleteReviewAsync(reviewId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteReview(reviewId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<List<string>>(notFoundResult.Value);
            Assert.Contains("Deletion failed", returnValue);
        }

        [Fact]
        public async Task GetReviewById_ReturnsOkResult_WhenReviewExists()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var mockResponse = new Response<ReviewResponseDto>
            {
                Data = new ReviewResponseDto { /* populate with necessary data */ },
                Succeeded = true
            };

            _reviewServiceMock
                .Setup(service => service.GetReviewByIdAsync(reviewId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetReviewById(reviewId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<ReviewResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetReviewById_ReturnsNotFound_WhenReviewDoesNotExist()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var mockResponse = new Response<ReviewResponseDto>
            {
                Succeeded = false,
                Errors = new List<string> { "Review not found" }
            };

            _reviewServiceMock
                .Setup(service => service.GetReviewByIdAsync(reviewId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetReviewById(reviewId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<List<string>>(notFoundResult.Value);
            Assert.Contains("Review not found", returnValue);
        }

        [Fact]
        public async Task GetReviewsForHostel_ReturnsOkResult_WhenReviewsExist()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var mockResponse = new Response<List<ReviewResponseDto>>
            {
                Data = new List<ReviewResponseDto> { /* populate with necessary data */ },
                Succeeded = true
            };

            _reviewServiceMock
                .Setup(service => service.GetReviewsForHostelAsync(hostelId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetReviewsForHostel(hostelId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<List<ReviewResponseDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetReviewsForHostel_ReturnsNotFound_WhenReviewsDoNotExist()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var mockResponse = new Response<List<ReviewResponseDto>>
            {
                Succeeded = false,
                Errors = new List<string> { "No reviews found for this hostel" }
            };

            _reviewServiceMock
                .Setup(service => service.GetReviewsForHostelAsync(hostelId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetReviewsForHostel(hostelId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<List<string>>(notFoundResult.Value);
            Assert.Contains("No reviews found for this hostel", returnValue);
        }

        [Fact]
        public async Task AddReview_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Rating", "Required");

            var reviewDto = new AddReviewRequestDto
            {
                HostelId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 0, // Invalid Rating
                Comment = "Great place!"
            };

            // Act
            var result = await _controller.AddReview(reviewDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task AddReview_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var reviewDto = new AddReviewRequestDto
            {
                HostelId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 5,
                Comment = "Great place!"
            };

            _reviewServiceMock
                .Setup(service => service.AddReviewAsync(reviewDto))
                .ThrowsAsync(new Exception("Something went wrong!"));

            // Act
            var result = await _controller.AddReview(reviewDto);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            Assert.Equal("Something went wrong!", internalServerErrorResult.Value);
        }

        [Theory]
        [InlineData(5, "Excellent place!")]
        [InlineData(3, "Average place")]
        [InlineData(1, "Bad experience")]
        public async Task AddReview_ReturnsOkResult_WithDifferentRatingsAndComments(int rating, string comment)
        {
            // Arrange
            var reviewDto = new AddReviewRequestDto
            {
                HostelId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = rating,
                Comment = comment
            };

            var mockResponse = new Response<ReviewResponseDto>
            {
                Data = new ReviewResponseDto { /* populate with necessary data */ },
                Succeeded = true
            };

            _reviewServiceMock
                .Setup(service => service.AddReviewAsync(reviewDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddReview(reviewDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<ReviewResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

    }
}
