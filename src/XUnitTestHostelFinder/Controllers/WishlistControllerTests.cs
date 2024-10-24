using HostelFinder.Application.DTOs.Wishlist.Request;
using HostelFinder.Application.DTOs.Wishlist.Response;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HostelFinder.UnitTests.Controllers
{
    public class WishlistControllerTests
    {
        private readonly WishlistController _controller;
        private readonly Mock<IWishlistService> _wishlistServiceMock;

        public WishlistControllerTests()
        {
            _wishlistServiceMock = new Mock<IWishlistService>();
            _controller = new WishlistController(_wishlistServiceMock.Object);
        }

        [Fact]
        public async Task AddPostToWishlist_ReturnsOkResult_WhenAdditionSucceeds()
        {
            // Arrange
            var request = new AddPostToWishlistRequestDto
            {
                PostId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var mockResponse = new Response<bool>
            {
                Data = true,
                Succeeded = true
            };

            _wishlistServiceMock
                .Setup(service => service.AddPostToWishlistAsync(request))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddRoomToWishlist(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetWishlistByUserId_ReturnsOkResult_WhenWishlistExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockResponse = new Response<WishlistResponseDto>
            {
                Data = new WishlistResponseDto { /* populate with necessary data */ },
                Succeeded = true
            };

            _wishlistServiceMock
                .Setup(service => service.GetWishlistByUserIdAsync(userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetWishlistByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<WishlistResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetWishlistByUserId_ReturnsNotFound_WhenWishlistDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockResponse = new Response<WishlistResponseDto>
            {
                Succeeded = false,
                Errors = new List<string> { "Wishlist not found" }
            };

            _wishlistServiceMock
                .Setup(service => service.GetWishlistByUserIdAsync(userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetWishlistByUserId(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<List<string>>(notFoundResult.Value);
            Assert.Contains("Wishlist not found", returnValue);
        }

        [Fact]
        public async Task DeleteRoomFromWishlist_ReturnsOkResult_WhenDeletionSucceeds()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Data = true,
                Succeeded = true
            };

            _wishlistServiceMock
                .Setup(service => service.DeleteRoomFromWishlistAsync(roomId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteWishlist(roomId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task DeleteRoomFromWishlist_ReturnsBadRequest_WhenDeletionFails()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Succeeded = false,
                Errors = new List<string> { "Deletion failed" }
            };

            _wishlistServiceMock
                .Setup(service => service.DeleteRoomFromWishlistAsync(roomId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteWishlist(roomId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<List<string>>(badRequestResult.Value);
            Assert.Contains("Deletion failed", returnValue);
        }

        [Fact]
        public async Task AddPostToWishlist_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            var request = new AddPostToWishlistRequestDto
            {
                PostId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _wishlistServiceMock
                .Setup(service => service.AddPostToWishlistAsync(request))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.AddRoomToWishlist(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var responseMessage = Assert.IsType<string>(objectResult.Value);
            Assert.Equal("Something went wrong!", responseMessage);
        }

    }
}
