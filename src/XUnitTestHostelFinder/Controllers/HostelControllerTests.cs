using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace XUnitTestHostelFinder.Controllers
{
    public class HostelControllerTests
    {
        private readonly Mock<IHostelService> _hostelServiceMock;
        private readonly Mock<IS3Service> _s3ServiceMock;
        private readonly HostelController _controller;

        public HostelControllerTests()
        {
            _hostelServiceMock = new Mock<IHostelService>();
            _s3ServiceMock = new Mock<IS3Service>();
            _controller = new HostelController(_hostelServiceMock.Object, _s3ServiceMock.Object);
        }

        [Fact]
        public async Task GetHostelById_ReturnsOkResult_WhenHostelExists()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var hostelResponse = new Response<HostelResponseDto>
            {
                Succeeded = true,
                Data = new HostelResponseDto { Id = hostelId, HostelName = "Test Hostel" }
            };
            _hostelServiceMock.Setup(s => s.GetHostelByIdAsync(hostelId))
                .ReturnsAsync(hostelResponse);

            // Act
            var result = await _controller.GetHostelById(hostelId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<HostelResponseDto>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.Equal(hostelId, response.Data.Id);
        }

        [Fact]
        public async Task GetHostelById_ReturnsNotFound_WhenHostelDoesNotExist()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var hostelResponse = new Response<HostelResponseDto> { Succeeded = false };
            _hostelServiceMock.Setup(s => s.GetHostelByIdAsync(hostelId))
                .ReturnsAsync(hostelResponse);

            // Act
            var result = await _controller.GetHostelById(hostelId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddHostel_ReturnsCreatedResult_WhenSuccessful()
        {
            // Arrange
            var addHostelRequest = new AddHostelRequestDto
            {
                HostelName = "Test Hostel",
                Description = "Test Description"
            };
            var images = new List<IFormFile>();
            var response = new Response<HostelResponseDto>
            {
                Succeeded = true,
                Data = new HostelResponseDto { Id = Guid.NewGuid(), HostelName = "Test Hostel" }
            };

            _hostelServiceMock.Setup(s => s.AddHostelAsync(addHostelRequest, It.IsAny<List<string>>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.AddHostel(addHostelRequest, images);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var responseData = Assert.IsType<Response<HostelResponseDto>>(createdResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal("Test Hostel", responseData.Data.HostelName);
        }

        [Fact]
        public async Task AddHostel_ReturnsBadRequest_WhenInvalidModel()
        {
            // Arrange
            _controller.ModelState.AddModelError("HostelName", "Required");

            // Act
            var result = await _controller.AddHostel(null, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateHostel_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var updateRequest = new UpdateHostelRequestDto
            {
                HostelName = "Updated Hostel",
                Description = "Updated Description"
            };
            var images = new List<IFormFile>();
            var response = new Response<HostelResponseDto>
            {
                Succeeded = true,
                Data = new HostelResponseDto { Id = hostelId, HostelName = "Updated Hostel" }
            };

            _hostelServiceMock.Setup(s => s.UpdateHostelAsync(hostelId, updateRequest, It.IsAny<List<string>>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateHostel(hostelId, updateRequest, images);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<HostelResponseDto>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal("Updated Hostel", responseData.Data.HostelName);
        }

        [Fact]
        public async Task DeleteHostel_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var response = new Response<bool> { Succeeded = true, Data = true, Message = "Delete successful." };

            _hostelServiceMock.Setup(s => s.DeleteHostelAsync(hostelId))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteHostel(hostelId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.True(responseData.Data);
            Assert.Equal("Delete successful.", responseData.Message);
        }

        [Fact]
        public async Task DeleteHostel_ReturnsNotFound_WhenFailed()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var response = new Response<bool> { Succeeded = false, Data = false, Message = "Hostel not found" };

            _hostelServiceMock.Setup(s => s.DeleteHostelAsync(hostelId))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteHostel(hostelId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var responseData = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.False(responseData.Data);
            Assert.Equal("Hostel not found", responseData.Message);
        }

        [Fact]
        public async Task AddHostel_ReturnsBadRequest_WhenHostelNameIsEmpty()
        {
            // Arrange
            var addHostelRequest = new AddHostelRequestDto
            {
                HostelName = "", // Invalid input
                Description = "Description"
            };
            _controller.ModelState.AddModelError("HostelName", "Hostel name is required.");

            // Act
            var result = await _controller.AddHostel(addHostelRequest, new List<IFormFile>());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task AddHostel_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var addHostelRequest = new AddHostelRequestDto
            {
                HostelName = "Test Hostel",
                Description = "Test Description"
            };

            _hostelServiceMock.Setup(s => s.AddHostelAsync(addHostelRequest, It.IsAny<List<string>>()))
                .ThrowsAsync(new Exception("Server error"));

            // Act
            var result = await _controller.AddHostel(addHostelRequest, new List<IFormFile>());

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Server error", statusCodeResult.Value);
        }

        [Theory]
        [InlineData("", "Test Description", false)] // Missing HostelName
        [InlineData("Test Hostel", "", false)]      // Missing Description
        [InlineData("Valid Hostel", "Valid Description", true)] // Valid input
        public async Task AddHostel_ReturnsExpectedResult(string hostelName, string description, bool expectedSuccess)
        {
            // Arrange
            var addHostelRequest = new AddHostelRequestDto
            {
                HostelName = hostelName,
                Description = description
            };

            var response = new Response<HostelResponseDto>
            {
                Succeeded = expectedSuccess,
                Data = expectedSuccess ? new HostelResponseDto { Id = Guid.NewGuid(), HostelName = hostelName } : null
            };

            if (!expectedSuccess)
            {
                _controller.ModelState.AddModelError("HostelName", "Hostel name is required.");
            }
            else
            {
                _hostelServiceMock.Setup(s => s.AddHostelAsync(addHostelRequest, It.IsAny<List<string>>()))
                    .ReturnsAsync(response);
            }

            // Act
            var result = await _controller.AddHostel(addHostelRequest, new List<IFormFile>());

            // Assert
            if (expectedSuccess)
            {
                var createdResult = Assert.IsType<CreatedAtActionResult>(result);
                var responseData = Assert.IsType<Response<HostelResponseDto>>(createdResult.Value);
                Assert.True(responseData.Succeeded);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequestResult.Value); // Ensure validation error returned
            }
        }

        [Fact]
        public async Task UpdateHostel_ReturnsBadRequest_WhenHostelNotFound()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var updateRequest = new UpdateHostelRequestDto
            {
                HostelName = "Updated Hostel",
                Description = "Updated Description"
            };

            var response = new Response<HostelResponseDto>
            {
                Succeeded = false,
                Message = "Hostel not found"
            };

            _hostelServiceMock.Setup(s => s.UpdateHostelAsync(hostelId, updateRequest, It.IsAny<List<string>>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateHostel(hostelId, updateRequest, new List<IFormFile>());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseData = Assert.IsType<Response<HostelResponseDto>>(badRequestResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("Hostel not found", responseData.Message);
        }

    }
}
