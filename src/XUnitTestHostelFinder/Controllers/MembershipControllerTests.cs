using HostelFinder.Application.DTOs.Membership.Requests;
using HostelFinder.Application.DTOs.Membership.Responses;
using HostelFinder.Application.DTOs.MembershipService.Requests;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HostelFinder.UnitTests.Controllers
{
    public class MembershipControllerTests
    {
        private readonly MembershipController _controller;
        private readonly Mock<IMembershipService> _membershipServiceMock;

        public MembershipControllerTests()
        {
            _membershipServiceMock = new Mock<IMembershipService>();
            //_controller = new MembershipController(_membershipServiceMock.Object);
        }

        [Fact]
        public async Task GetListMembership_ReturnsOkResult_WhenMembershipsExist()
        {
            // Arrange
            var mockResponse = new Response<List<MembershipResponseDto>>
            {
                Data = new List<MembershipResponseDto>
                {
                    new MembershipResponseDto { /* populate with necessary data */ },
                    new MembershipResponseDto { /* populate with necessary data */ }
                },
                Succeeded = true
            };

            _membershipServiceMock
                .Setup(service => service.GetAllMembershipWithMembershipService())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetListMembership();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<List<MembershipResponseDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetListMembership_ReturnsNotFound_WhenNoMembershipsExist()
        {
            // Arrange
            var mockResponse = new Response<List<MembershipResponseDto>>
            {
                Data = null,
                Succeeded = false,
                Errors = new List<string> { "No memberships found." }
            };

            _membershipServiceMock
                .Setup(service => service.GetAllMembershipWithMembershipService())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetListMembership();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<List<string>>(notFoundResult.Value);
            Assert.Contains("No memberships found.", returnValue);
        }

        [Fact]
        public async Task AddMembership_ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            _membershipServiceMock
                .Setup(service => service.AddMembershipAsync(It.IsAny<AddMembershipRequestDto>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var membershipDto = new AddMembershipRequestDto { Name = "Basic" };

            // Act
            var result = await _controller.AddMembership(membershipDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode); // HTTP 500
        }

        [Fact]
        public async Task AddMembership_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            var membershipDto = new AddMembershipRequestDto(); // Dữ liệu không hợp lệ

            // Act
            var result = await _controller.AddMembership(membershipDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value); // Kiểm tra lỗi từ ModelState
        }


        [Fact]
        public async Task AddMembership_ReturnsOkResult_WhenAdditionSucceeds()
        {
            // Arrange
            var membershipDto = new AddMembershipRequestDto
            {
                // Populate with necessary data
            };

            var mockResponse = new Response<MembershipResponseDto>
            {
                Data = new MembershipResponseDto { /* populate with necessary data */ },
                Succeeded = true
            };

            _membershipServiceMock
                .Setup(service => service.AddMembershipAsync(membershipDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddMembership(membershipDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<MembershipResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task AddMembership_ReturnsBadRequest_WhenAdditionFails()
        {
            // Arrange
            var membershipDto = new AddMembershipRequestDto
            {
                // Populate with necessary data
            };

            var mockResponse = new Response<MembershipResponseDto>
            {
                Succeeded = false,
                Errors = new List<string> { "Addition failed" }
            };

            _membershipServiceMock
                .Setup(service => service.AddMembershipAsync(membershipDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddMembership(membershipDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<List<string>>(badRequestResult.Value);
            Assert.Contains("Addition failed", returnValue);
        }

        [Theory]
        [InlineData("Membership A", "Description A", true)] // Thí dụ hợp lệ
        [InlineData("", "Description B", false)] // Tên thành viên trống
        [InlineData("Membership C", "", false)] // Mô tả thành viên trống
        public async Task AddMembership_WithDifferentData_ReturnsExpectedResult(string membershipName, string description, bool isExpectedSuccess)
        {
            // Arrange
            var membershipDto = new AddMembershipRequestDto
            {
                Name = membershipName,
                Description = description
            };

            var mockResponse = isExpectedSuccess
                ? new Response<MembershipResponseDto>
                {
                    Data = new MembershipResponseDto { Name = membershipName },
                    Succeeded = true
                }
                : new Response<MembershipResponseDto>
                {
                    Data = null,
                    Succeeded = false,
                    Errors = new List<string> { "Invalid membership data" }
                };

            _membershipServiceMock
                .Setup(service => service.AddMembershipAsync(It.IsAny<AddMembershipRequestDto>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddMembership(membershipDto);

            // Assert
            if (isExpectedSuccess)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<Response<MembershipResponseDto>>(okResult.Value);
                Assert.True(returnValue.Succeeded);
                Assert.Equal(membershipName, returnValue.Data.Name);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var returnValue = Assert.IsType<List<string>>(badRequestResult.Value);
                Assert.Contains("Invalid membership data", returnValue);
            }
        }


        [Fact]
        public async Task EditMembership_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var membershipId = Guid.NewGuid();
            var updateDto = new UpdateMembershipRequestDto
            {
                Name = "Updated Membership",
                Description = "Updated Description",
                Price = 100.0m,
                Duration = 30,
                MembershipServices = new List<UpdateMembershipServiceReqDto>
        {
            new UpdateMembershipServiceReqDto
            {
                Id = Guid.NewGuid(),
                ServiceName = "New Service",
                MaxPostsAllowed = 10,
                MaxPushTopAllowed = 5
            }
        }
            };

            var response = new Response<MembershipResponseDto>
            {
                Succeeded = true,
                Data = new MembershipResponseDto
                {
                    Name = updateDto.Name,
                    Description = updateDto.Description,
                    Price = updateDto.Price,
                    Duration = updateDto.Duration
                }
            };

            _membershipServiceMock.Setup(service => service.EditMembershipAsync(membershipId, updateDto))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.EditMembership(membershipId, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<MembershipResponseDto>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal("Updated Membership", responseData.Data.Name);
        }

        [Fact]
        public async Task EditMembership_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidDto = new UpdateMembershipRequestDto(); // Missing required fields

            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.EditMembership(Guid.NewGuid(), invalidDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var modelStateErrors = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelStateErrors.ContainsKey("Name"));
        }


        [Fact]
        public async Task EditMembership_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var membershipId = Guid.NewGuid();
            var updateDto = new UpdateMembershipRequestDto
            {
                Name = "Updated Membership",
                Description = "Updated Description",
                Price = 100.0m,
                Duration = 30
            };

            _membershipServiceMock.Setup(service => service.EditMembershipAsync(membershipId, updateDto))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.EditMembership(membershipId, updateDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var responseMessage = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.False(responseMessage.Succeeded);
            Assert.Equal("Internal server error: Internal server error", responseMessage.Message);
        }


        [Fact]
        public async Task EditMembership_ReturnsBadRequest_WhenMembershipNotFound()
        {
            // Arrange
            var membershipId = Guid.NewGuid();
            var updateDto = new UpdateMembershipRequestDto
            {
                Name = "Updated Membership",
                Description = "Updated Description",
                Price = 100.0m,
                Duration = 30
            };

            var response = new Response<MembershipResponseDto>
            {
                Succeeded = false,
                Message = "Membership not found."
            };

            _membershipServiceMock.Setup(service => service.EditMembershipAsync(membershipId, updateDto))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.EditMembership(membershipId, updateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Membership not found.", responseMessage);
        }

        [Fact]
        public async Task DeleteMembership_ReturnsOkResult_WhenDeletionSucceeds()
        {
            // Arrange
            var membershipId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Succeeded = true,
                Message = "Membership deleted successfully."
            };

            _membershipServiceMock
                .Setup(service => service.DeleteMembershipAsync(membershipId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteMembership(membershipId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.Equal("Membership deleted successfully.", response.Message);
        }

        [Fact]
        public async Task DeleteMembership_ReturnsNotFound_WhenMembershipDoesNotExist()
        {
            // Arrange
            var membershipId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Succeeded = false,
                Message = "Membership not found."
            };

            _membershipServiceMock
                .Setup(service => service.DeleteMembershipAsync(membershipId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteMembership(membershipId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Membership not found.", response.Message);
        }

        [Fact]
        public async Task DeleteMembership_ReturnsBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = Guid.Empty;

            // Act
            var result = await _controller.DeleteMembership(invalidId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Invalid membership ID.", response.Message);
        }

        [Fact]
        public async Task DeleteMembership_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var membershipId = Guid.NewGuid();

            _membershipServiceMock
                .Setup(service => service.DeleteMembershipAsync(membershipId))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.DeleteMembership(membershipId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var response = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Internal server error: Internal server error", response.Message);
        }

        [Fact]
        public async Task AddUserMembership_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidRequest = new AddUserMembershipRequestDto();
            _controller.ModelState.AddModelError("UserId", "The UserId field is required.");

            // Act
            var result = await _controller.AddUserMembership(invalidRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Invalid request model.", response.Message);
        }

        [Fact]
        public async Task AddUserMembership_ReturnsBadRequest_WhenUserAlreadyAssignedToMembership()
        {
            // Arrange
            var request = new AddUserMembershipRequestDto
            {
                UserId = Guid.NewGuid(),
                MembershipId = Guid.NewGuid()
            };

            var mockResponse = new Response<string>
            {
                Succeeded = false,
                Message = "User is already assigned to this membership."
            };

            _membershipServiceMock.Setup(service => service.AddUserMembershipAsync(request))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddUserMembership(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("User is already assigned to this membership.", response.Message);
        }

        [Fact]
        public async Task AddUserMembership_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var request = new AddUserMembershipRequestDto
            {
                UserId = Guid.NewGuid(),
                MembershipId = Guid.NewGuid()
            };

            var mockResponse = new Response<string>
            {
                Succeeded = true,
                Data = "User membership added successfully."
            };

            _membershipServiceMock.Setup(service => service.AddUserMembershipAsync(request))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddUserMembership(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<string>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.Equal("User membership added successfully.", response.Data);
        }

        [Fact]
        public async Task AddUserMembership_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var request = new AddUserMembershipRequestDto
            {
                UserId = Guid.NewGuid(),
                MembershipId = Guid.NewGuid()
            };

            _membershipServiceMock.Setup(service => service.AddUserMembershipAsync(request))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.AddUserMembership(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var response = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Internal server error: Internal server error", response.Message);
        }

    }
}
