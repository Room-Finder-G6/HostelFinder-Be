using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.Room.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Enums;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestHostelFinder.Controllers
{
    public class RoomControllerTests
    {
        private readonly Mock<IRoomService> _roomServiceMock;
        private readonly RoomController _controller;

        public RoomControllerTests()
        {
            _roomServiceMock = new Mock<IRoomService>();
            _controller = new RoomController(_roomServiceMock.Object);
        }

        // 1. Test Bad Request for Invalid Model in CreateRoom
        [Fact]
        public async Task CreateRoom_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("RoomName", "RoomName is required");

            // Act
            var result = await _controller.CreateRoom(new AddRoomRequestDto());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task CreateRoom_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            var roomDto = new AddRoomRequestDto { RoomName = "Room A", RoomType = RoomType.Chung_cư };

            _roomServiceMock
                .Setup(service => service.CreateAsync(It.IsAny<AddRoomRequestDto>()))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.CreateRoom(roomDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error: Internal server error", objectResult.Value);
        }

        [Fact]
        public async Task UpdateRoom_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var roomDto = new UpdateRoomRequestDto { RoomName = "Updated Room" };
            _roomServiceMock
                .Setup(service => service.UpdateAsync(roomId, roomDto))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.UpdateRoom(roomId, roomDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error: Internal server error", objectResult.Value);
        }


        // 4. Parameterized Test for CreateRoom with Different Data Scenarios
        [Theory]
        [InlineData("Room A", RoomType.Chung_cư, true)]   // Valid data
        [InlineData("", RoomType.Phòng_trọ, false)]       // Invalid: Empty RoomName
        [InlineData("Room C", RoomType.Chung_cư_mini, true)]  // Another valid case
        public async Task CreateRoom_WithDifferentData_ReturnsExpectedResult(string roomName, RoomType roomType, bool shouldSucceed)
        {
            // Arrange
            var roomDto = new AddRoomRequestDto
            {
                RoomName = roomName,
                RoomType = roomType
            };

            var mockResponse = new Response<RoomResponseDto>
            {
                Data = shouldSucceed ? new RoomResponseDto { RoomName = roomName, RoomType = roomType } : null,
                Succeeded = shouldSucceed,
                Message = shouldSucceed ? "Room created successfully" : "Invalid room data"
            };

            _roomServiceMock
                .Setup(service => service.CreateAsync(roomDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CreateRoom(roomDto);

            // Assert
            if (shouldSucceed)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<RoomResponseDto>(okResult.Value);
                Assert.Equal(roomName, returnValue.RoomName);
                Assert.Equal(roomType, returnValue.RoomType);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid room data", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task DeleteRoom_ReturnsBadRequest_WhenRoomIdIsInvalid()
        {
            // Arrange
            var invalidRoomId = Guid.Empty;
            var mockResponse = new Response<bool>
            {
                Succeeded = false,
                Message = "Invalid room ID"
            };

            _roomServiceMock
                .Setup(service => service.DeleteAsync(invalidRoomId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteRoom(invalidRoomId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid room ID", badRequestResult.Value);
        }

        // 1. Test GetRooms Success
        [Fact]
        public async Task GetRooms_ReturnsOkResult_WhenRoomsExist()
        {
            // Arrange
            var mockResponse = new Response<List<RoomResponseDto>>
            {
                Data = new List<RoomResponseDto> { new RoomResponseDto { RoomName = "Room 101" } },
                Succeeded = true
            };

            _roomServiceMock
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetRooms();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<RoomResponseDto>>(okResult.Value);
            Assert.NotEmpty(returnValue);
        }

        // 2. Test GetRooms Failure
        [Fact]
        public async Task GetRooms_ReturnsBadRequest_WhenGetAllFails()
        {
            // Arrange
            var mockResponse = new Response<List<RoomResponseDto>>
            {
                Succeeded = false,
                Message = "Failed to retrieve rooms."
            };

            _roomServiceMock
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetRooms();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to retrieve rooms.", badRequestResult.Value);
        }

        // 3. Test GetRoom Success
        [Fact]
        public async Task GetRoom_ReturnsOkResult_WhenRoomExists()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var mockResponse = new Response<RoomResponseDto>
            {
                Data = new RoomResponseDto { RoomName = "Room 101" },
                Succeeded = true
            };

            _roomServiceMock
                .Setup(service => service.GetByIdAsync(roomId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetRoom(roomId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<RoomResponseDto>(okResult.Value);
            Assert.Equal("Room 101", returnValue.RoomName);
        }

        // 4. Test GetRoom Not Found
        [Fact]
        public async Task GetRoom_ReturnsNotFound_WhenRoomDoesNotExist()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var mockResponse = new Response<RoomResponseDto>
            {
                Succeeded = false,
                Message = "Room not found."
            };

            _roomServiceMock
                .Setup(service => service.GetByIdAsync(roomId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetRoom(roomId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Room not found.", notFoundResult.Value);
        }

        // 5. Test CreateRoom Success
        [Fact]
        public async Task CreateRoom_ReturnsOkResult_WhenRoomIsCreated()
        {
            // Arrange
            var roomDto = new AddRoomRequestDto { RoomName = "Room 101" };
            var mockResponse = new Response<RoomResponseDto>
            {
                Data = new RoomResponseDto { RoomName = "Room 101" },
                Succeeded = true
            };

            _roomServiceMock
                .Setup(service => service.CreateAsync(roomDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CreateRoom(roomDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<RoomResponseDto>(okResult.Value);
            Assert.Equal("Room 101", returnValue.RoomName);
        }

        // 7. Test UpdateRoom Success
        [Fact]
        public async Task UpdateRoom_ReturnsOkResult_WhenUpdateSucceeds()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var roomDto = new UpdateRoomRequestDto { RoomName = "Updated Room" };
            var mockResponse = new Response<RoomResponseDto>
            {
                Data = new RoomResponseDto { RoomName = "Updated Room" },
                Succeeded = true
            };

            _roomServiceMock
                .Setup(service => service.UpdateAsync(roomId, roomDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateRoom(roomId, roomDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<RoomResponseDto>(okResult.Value);
            Assert.Equal("Updated Room", returnValue.RoomName);
        }

        // 8. Test UpdateRoom Not Found
        [Fact]
        public async Task UpdateRoom_ReturnsNotFound_WhenRoomDoesNotExist()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var roomDto = new UpdateRoomRequestDto { RoomName = "Updated Room" };
            var mockResponse = new Response<RoomResponseDto>
            {
                Succeeded = false,
                Message = "Room not found."
            };

            _roomServiceMock
                .Setup(service => service.UpdateAsync(roomId, roomDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateRoom(roomId, roomDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Room not found.", notFoundResult.Value);
        }

        // 9. Test DeleteRoom Success
        [Fact]
        public async Task DeleteRoom_ReturnsOkResult_WhenDeleteSucceeds()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Data = true,
                Succeeded = true
            };

            _roomServiceMock
                .Setup(service => service.DeleteAsync(roomId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteRoom(roomId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(returnValue.Data);
        }

        // 10. Test DeleteRoom Not Found
        [Fact]
        public async Task DeleteRoom_ReturnsNotFound_WhenRoomDoesNotExist()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Succeeded = false,
                Message = "Room not found."
            };

            _roomServiceMock
                .Setup(service => service.DeleteAsync(roomId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteRoom(roomId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Room not found.", notFoundResult.Value);
        }
    }
}
