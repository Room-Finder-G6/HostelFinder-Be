using HostelFinder.Application.DTOs.Membership.Requests;
using HostelFinder.Application.DTOs.Membership.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace HostelFinder.UnitTests.Controllers
{
    public class MembershipControllerTests
    {
        private readonly MembershipController _controller;
        private readonly Mock<IMembershipService> _membershipServiceMock;

        public MembershipControllerTests()
        {
            _membershipServiceMock = new Mock<IMembershipService>();
            _controller = new MembershipController(_membershipServiceMock.Object);
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
        public async Task EditMembership_ReturnsOkResult_WhenUpdateSucceeds()
        {
            // Arrange
            var membershipDto = new UpdateMembershipRequestDto
            {
                // Populate with necessary data
            };
            var membershipId = Guid.NewGuid();

            var mockResponse = new Response<MembershipResponseDto>
            {
                Data = new MembershipResponseDto { /* populate with necessary data */ },
                Succeeded = true
            };

            _membershipServiceMock
                .Setup(service => service.EditMembershipAsync(membershipId, membershipDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.EditMembership(membershipId, membershipDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<MembershipResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task EditMembership_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var membershipDto = new UpdateMembershipRequestDto
            {
                // Populate with necessary data
            };
            var membershipId = Guid.NewGuid();

            var mockResponse = new Response<MembershipResponseDto>
            {
                Succeeded = false,
                Errors = new List<string> { "Update failed" }
            };

            _membershipServiceMock
                .Setup(service => service.EditMembershipAsync(membershipId, membershipDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.EditMembership(membershipId, membershipDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<List<string>>(badRequestResult.Value);
            Assert.Contains("Update failed", returnValue);
        }

        [Fact]
        public async Task DeleteMembership_ReturnsOkResult_WhenDeletionSucceeds()
        {
            // Arrange
            var membershipId = Guid.NewGuid();
            var mockResponse = new Response<string>
            {
                Data = "Membership deleted successfully",
                Succeeded = true
            };

            _membershipServiceMock
                .Setup(service => service.DeleteMembershipAsync(membershipId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteMembership(membershipId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<string>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task DeleteMembership_ReturnsBadRequest_WhenDeletionFails()
        {
            // Arrange
            var membershipId = Guid.NewGuid();
            var mockResponse = new Response<string>
            {
                Succeeded = false,
                Errors = new List<string> { "Deletion failed" }
            };

            _membershipServiceMock
                .Setup(service => service.DeleteMembershipAsync(membershipId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteMembership(membershipId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<List<string>>(badRequestResult.Value);
            Assert.Contains("Deletion failed", returnValue);
        }
    }
}
