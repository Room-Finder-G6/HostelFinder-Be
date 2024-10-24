using HostelFinder.Application.DTOs.Users;
using HostelFinder.Application.DTOs.Users.Requests;
using HostelFinder.Application.DTOs.Users.Response;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace XUnitTestHostelFinder.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task GetListUser_ReturnsOkResult_WhenUsersExist()
        {
            // Arrange
            var mockResponse = new Response<List<UserDto>>
            {
                Data = new List<UserDto>
        {
            new UserDto { Username = "user1", Email = "user1@example.com", Phone = "123456789", IsActive = true },
            new UserDto { Username = "user2", Email = "user2@example.com", Phone = "987654321", IsActive = true }
        },
                Succeeded = true
            };

            _userServiceMock
                .Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetListUser();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<List<UserDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.Equal(2, returnValue.Data.Count);
        }

        [Fact]
        public async Task GetListUser_ReturnsNotFound_WhenNoUsersExist()
        {
            // Arrange
            var mockResponse = new Response<List<UserDto>>
            {
                Data = null,
                Succeeded = false,
                Errors = new List<string> { "No users found" }
            };

            _userServiceMock
                .Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetListUser();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Response<List<UserDto>>>(notFoundResult.Value);
            Assert.False(returnValue.Succeeded);
            Assert.Contains("No users found", returnValue.Errors);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockResponse = new Response<UserProfileResponse>
            {
                Data = new UserProfileResponse
                {
                    Id = userId,
                    Username = "user1",
                    Email = "user1@example.com",
                    Phone = "123456789",
                    AvatarUrl = "avatar.jpg"
                },
                Succeeded = true
            };

            _userServiceMock
                .Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<UserProfileResponse>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.Equal("user1", returnValue.Data.Username);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userServiceMock
                .Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync((Response<UserProfileResponse>)null); 

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Null(notFoundResult.Value); 
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkResult_WhenUpdateSucceeds()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updatedUser = new UserDto
            {
                Username = "updatedUsername",
                Email = "updated@example.com",
                Phone = "123456789",
                AvatarUrl = null,
                Role = HostelFinder.Domain.Enums.UserRole.User,
                IsActive = true
            };

            var mockResponse = new Response<UserDto>
            {
                Data = updatedUser,
                Succeeded = true
            };

            _userServiceMock
                .Setup(service => service.UpdateUserAsync(userId, It.IsAny<UpdateUserRequestDto>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateUser(userId, new UpdateUserRequestDto());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<UserDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.Equal("updatedUsername", returnValue.Data.Username);
            Assert.Equal("updated@example.com", returnValue.Data.Email);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNotFound_WhenUpdateFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateUserDto = new UpdateUserRequestDto
            {
                Username = "updatedUser",
                Email = "updated@example.com",
                Phone = "123456789"
            };

            var mockResponse = new Response<UserDto>
            {
                Succeeded = false,
                Errors = new List<string> { "Update failed" }
            };

            _userServiceMock
                .Setup(service => service.UpdateUserAsync(userId, updateUserDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateUser(userId, updateUserDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); 
            var returnValue = Assert.IsType<Response<UserDto>>(notFoundResult.Value);
            Assert.False(returnValue.Succeeded);
            Assert.Contains("Update failed", returnValue.Errors);
        }


        [Fact]
        public async Task UnActiveUser_ReturnsOkResult_WhenDeactivationSucceeds()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Data = true,
                Succeeded = true
            };

            _userServiceMock
                .Setup(service => service.UnActiveUserAsync(userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UnActiveUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.True(returnValue.Data);
        }


        [Fact]
        public async Task UnActiveUser_ReturnsNotFound_WhenDeactivationFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Data = false,
                Succeeded = false,
                Errors = new List<string> { "User deactivation failed" }
            };

            _userServiceMock
                .Setup(service => service.UnActiveUserAsync(userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UnActiveUser(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); 
            var returnValue = Assert.IsType<Response<bool>>(notFoundResult.Value); 
            Assert.False(returnValue.Succeeded);
            Assert.Contains("User deactivation failed", returnValue.Errors);
        }

        [Fact]
        public async Task GetListUser_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            _userServiceMock
                .Setup(service => service.GetAllUsersAsync())
                .ThrowsAsync(new Exception("Something went wrong!"));

            // Act
            var result = await _controller.GetListUser();

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            Assert.Equal("Something went wrong!", internalServerErrorResult.Value);
        }

        [Fact]
        public async Task GetUserById_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Id", "Invalid user ID");

            // Act
            var result = await _controller.GetUserById(Guid.Empty); // Simulate an invalid Guid

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateUserDto = new UpdateUserRequestDto
            {
                Username = "updatedUser",
                Email = "updated@example.com",
                Phone = "123456789"
            };

            _userServiceMock
                .Setup(service => service.UpdateUserAsync(userId, updateUserDto))
                .ThrowsAsync(new Exception("Something went wrong!"));

            // Act
            var result = await _controller.UpdateUser(userId, updateUserDto);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            Assert.Equal("Something went wrong!", internalServerErrorResult.Value);
        }

        [Fact]
        public async Task UnActiveUser_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Id", "Invalid user ID");

            // Act
            var result = await _controller.UnActiveUser(Guid.Empty); // Simulate an invalid Guid

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

    }
}
