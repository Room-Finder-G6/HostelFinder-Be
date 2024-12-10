using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.Room.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace XUnitTestHostelFinder.Controllers
{
    public class RoomControllerTests
    {
        private readonly Mock<IRoomService> _roomServiceMock;
        private readonly Mock<ITenantService> _tenantServiceMock;
        private readonly Mock<IRoomTenancyService> _roomTenacyServiceMock;
        private readonly RoomController _controller;

        public RoomControllerTests()
        {
            _roomServiceMock = new Mock<IRoomService>();
            _controller = new RoomController(_roomServiceMock.Object, _tenantServiceMock.Object, _roomTenacyServiceMock.Object);

        }

        [Fact]
        public async Task CreateRoom_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var roomDto = new AddRoomRequestDto
            {
                HostelId = Guid.NewGuid(),
                RoomName = "Room A",
                Floor = 2,
                MaxRenters = 3,
                Deposit = 500,
                MonthlyRentCost = 1200,
                Size = 25,
                RoomType = RoomType.Phòng_trọ,
                AmenityId = new List<Guid> { Guid.NewGuid() }
            };
            var mockResponse = new Response<RoomResponseDto>
            {
                Succeeded = true,
                Message = "Thêm phòng trọ thành công"
            };

            _roomServiceMock.Setup(service => service.CreateRoomAsync(roomDto, It.IsAny<List<IFormFile>>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CreateRoom(roomDto, new List<IFormFile>());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<RoomResponseDto>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal("Thêm phòng trọ thành công", responseData.Message);
        }

        [Fact]
        public async Task CreateRoom_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("RoomName", "RoomName is required");

            // Act
            var result = await _controller.CreateRoom(new AddRoomRequestDto(), new List<IFormFile>());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelState.ContainsKey("RoomName"));
        }

        [Fact]
        public async Task CreateRoom_ReturnsBadRequest_WhenRoomNameAlreadyExists()
        {
            // Arrange
            var roomDto = new AddRoomRequestDto
            {
                HostelId = Guid.NewGuid(),
                RoomName = "Existing Room",
                Floor = 1,
                MaxRenters = 4,
                Deposit = 500,
                MonthlyRentCost = 1000,
                Size = 20,
                RoomType = RoomType.Chung_cư_mini,
                AmenityId = new List<Guid> { Guid.NewGuid() }
            };
            var mockResponse = new Response<RoomResponseDto>
            {
                Succeeded = false,
                Message = "Tên phòng đã tồn tại trong trọ."
            };

            _roomServiceMock.Setup(service => service.CreateRoomAsync(roomDto, It.IsAny<List<IFormFile>>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CreateRoom(roomDto, new List<IFormFile>());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseData = Assert.IsType<Response<RoomResponseDto>>(badRequestResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("Tên phòng đã tồn tại trong trọ.", responseData.Message);
        }

        [Fact]
        public async Task CreateRoom_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var roomDto = new AddRoomRequestDto
            {
                HostelId = Guid.NewGuid(),
                RoomName = "Room B",
                Floor = 2,
                MaxRenters = 3,
                Deposit = 600,
                MonthlyRentCost = 1500,
                Size = 30,
                RoomType = RoomType.Chung_cư,
                AmenityId = new List<Guid> { Guid.NewGuid() }
            };

            _roomServiceMock.Setup(service => service.CreateRoomAsync(roomDto, It.IsAny<List<IFormFile>>()))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.CreateRoom(roomDto, new List<IFormFile>());

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
            var images = new List<IFormFile>();
            _roomServiceMock
                .Setup(service => service.UpdateAsync(roomId, roomDto, images))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.UpdateRoom(roomId, roomDto,images);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error: Internal server error", objectResult.Value);
        }


        // 4. Parameterized Test for CreateRoom with Different Data Scenarios
        [Theory]
        [InlineData("Room A", 2, 3, 1200, 25, true, "Thêm phòng trọ thành công", 200)] // Valid case
        [InlineData("Existing Room", 1, 4, 1000, 20, false, "Tên phòng đã tồn tại trong trọ.", 400)] // Duplicate room name
        [InlineData("", 0, 0, 0, 0, false, "Model is invalid", 400)] // Invalid input case
        [InlineData(null, 3, 2, 500, 15, false, "Model is invalid", 400)] // Missing RoomName
        public async Task CreateRoom_WithDifferentData_ReturnsExpectedResult(
     string roomName, int floor, int maxRenters, decimal monthlyRentCost, decimal size,
     bool success, string message, int expectedStatusCode)
        {
            // Arrange
            var roomDto = new AddRoomRequestDto
            {
                HostelId = Guid.NewGuid(),
                RoomName = roomName,
                Floor = floor,
                MaxRenters = maxRenters,
                Deposit = 500,
                MonthlyRentCost = monthlyRentCost,
                Size = size,
                RoomType = RoomType.Chung_cư,
                AmenityId = new List<Guid> { Guid.NewGuid() }
            };

            var mockResponse = new Response<RoomResponseDto>
            {
                Succeeded = success,
                Message = message
            };

            if (roomName == "Existing Room")
            {
                _roomServiceMock.Setup(service => service.CreateRoomAsync(roomDto, It.IsAny<List<IFormFile>>()))
                    .ReturnsAsync(mockResponse);
            }
            else if (string.IsNullOrEmpty(roomName))
            {
                _controller.ModelState.AddModelError("RoomName", "RoomName is required");
            }
            else
            {
                _roomServiceMock.Setup(service => service.CreateRoomAsync(roomDto, It.IsAny<List<IFormFile>>()))
                    .ReturnsAsync(mockResponse);
            }

            // Act
            var result = await _controller.CreateRoom(roomDto, new List<IFormFile>());

            // Assert
            if (expectedStatusCode == 200)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<Response<RoomResponseDto>>(okResult.Value);
                Assert.Equal(success, returnValue.Succeeded);
                Assert.Equal(message, returnValue.Message);
            }
            else if (expectedStatusCode == 400 && string.IsNullOrEmpty(roomName))
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
                Assert.True(modelState.ContainsKey("RoomName"));
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var responseData = Assert.IsType<Response<RoomResponseDto>>(badRequestResult.Value);
                Assert.False(responseData.Succeeded);
                Assert.Equal(message, responseData.Message);
            }
        }


        [Fact]
        public async Task DeleteRoom_ReturnsBadRequest_WhenRoomIdIsInvalid()
        {
            // Arrange
            var invalidRoomId = Guid.Empty;

            // Act
            var result = await _controller.DeleteRoom(invalidRoomId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Invalid room ID", response.Message);
        }


        [Fact]
        public async Task GetRooms_ReturnsOkResult_WhenRoomsExist()
        {
            // Arrange
            var mockRooms = new List<RoomResponseDto>
    {
        new RoomResponseDto { Id = Guid.NewGuid(), RoomName = "Room A", MonthlyRentCost = 1000 },
        new RoomResponseDto { Id = Guid.NewGuid(), RoomName = "Room B", MonthlyRentCost = 1500 }
    };

            var mockResponse = new Response<List<RoomResponseDto>>(mockRooms);

            _roomServiceMock.Setup(service => service.GetAllAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetRooms();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<List<RoomResponseDto>>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal(2, responseData.Data.Count);
        }


        [Fact]
        public async Task GetRooms_ReturnsBadRequest_WhenGetAllFails()
        {
            // Arrange
            var mockResponse = new Response<List<RoomResponseDto>>
            {
                Succeeded = false,
                Message = "Service failure"
            };

            _roomServiceMock.Setup(service => service.GetAllAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetRooms();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseData = Assert.IsType<Response<List<RoomResponseDto>>>(badRequestResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("Service failure", responseData.Message);
        }


        [Fact]
        public async Task GetRooms_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _roomServiceMock.Setup(service => service.GetAllAsync())
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.GetRooms();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var responseData = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("Internal server error: Internal server error", responseData.Message);
        }


        [Fact]
        public async Task GetRooms_ReturnsEmptyList_WhenNoRoomsExist()
        {
            // Arrange
            var mockResponse = new Response<List<RoomResponseDto>>(new List<RoomResponseDto>(), "No rooms found")
            {
                Succeeded = true
            };

            _roomServiceMock.Setup(service => service.GetAllAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetRooms();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<List<RoomResponseDto>>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Empty(responseData.Data);
            Assert.Equal("No rooms found", responseData.Message);
        }


        // 3. Test GetRoom Success
        [Fact]
        public async Task GetRoom_ReturnsOkResult_WhenRoomExists()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var roomResponse = new RoomByIdDto()
            {
                Id = roomId,
                HostelName = "Hostel A",
                RoomName = "Room 101",
                Floor = 1,
                MaxRenters = 3,
                Size = 25.5f,
                IsAvailable = true,
                MonthlyRentCost = 500,
                RoomType = RoomType.Chung_cư,
                CreatedOn = DateTimeOffset.Now,
                ImageRoom =  new List<string>()
                {
                    "http://example.com/room101.jpg",
                    "http://example.com/room101_2.jpg"
                }
            };
            var response = new Response<RoomByIdDto>(roomResponse);

            _roomServiceMock.Setup(service => service.GetByIdAsync(roomId))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetRoom(roomId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<RoomResponseDto>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.NotNull(responseData.Data);
            Assert.Equal("Room 101", responseData.Data.RoomName);
        }

        // 4. Test GetRoom Not Found
        [Fact]
        public async Task GetRoom_ReturnsNotFound_WhenRoomDoesNotExist()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var response = new Response<RoomByIdDto>("Room not found.")
            {
                Succeeded = false
            };

            _roomServiceMock.Setup(service => service.GetByIdAsync(roomId))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetRoom(roomId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var message = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal("Room not found.", message);
        }

        [Fact]
        public async Task GetRoom_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            _roomServiceMock.Setup(service => service.GetByIdAsync(roomId))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.GetRoom(roomId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error: Internal server error", objectResult.Value);
        }

        // 7. Test UpdateRoom Success
        [Fact]
        public async Task UpdateRoom_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var updateDto = new UpdateRoomRequestDto
            {
                RoomName = "Updated Room",
                MonthlyRentCost = 1200,
                RoomType = RoomType.Chung_cư_mini
            };
            var roomResponse = new RoomResponseDto
            {
                Id = roomId,
                RoomName = "Updated Room",
                IsAvailable = true,
                MonthlyRentCost = 1200,
                RoomType = RoomType.Chung_cư_mini
            };
            var images = new List<IFormFile>();
            var response = new Response<RoomResponseDto>(roomResponse, "Room updated successfully.");

            _roomServiceMock.Setup(service => service.UpdateAsync(roomId, updateDto, images))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateRoom(roomId, updateDto, images);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<RoomResponseDto>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal("Room updated successfully.", responseData.Message);
        }

        [Fact]
        public async Task UpdateRoom_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("RoomName", "RoomName is required");
            var images = new List<IFormFile>();
            // Act
            var result = await _controller.UpdateRoom(Guid.NewGuid(), new UpdateRoomRequestDto(), images);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelState.ContainsKey("RoomName"));
        }

        // 8. Test UpdateRoom Not Found
        [Fact]
        public async Task UpdateRoom_ReturnsBadRequest_WhenRoomNotFound()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var updateDto = new UpdateRoomRequestDto
            {
                RoomName = "Updated Room",
                MonthlyRentCost = 1200,
                RoomType = RoomType.Chung_cư_mini
            };
            var images = new List<IFormFile>();
            var response = new Response<RoomResponseDto>("Room not found.")
            {
                Succeeded = false
            };

            _roomServiceMock.Setup(service => service.UpdateAsync(roomId, updateDto, images))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateRoom(roomId, updateDto, images);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseData = Assert.IsType<Response<RoomResponseDto>>(badRequestResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("Room not found.", responseData.Message);
        }

        [Fact]
        public async Task UpdateRoom_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var updateDto = new UpdateRoomRequestDto
            {
                RoomName = "Updated Room",
                MonthlyRentCost = 1200,
                RoomType = RoomType.Chung_cư_mini
            };
            var images = new List<IFormFile>();

            _roomServiceMock.Setup(service => service.UpdateAsync(roomId, updateDto, images))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.UpdateRoom(roomId, updateDto, images);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error: Internal server error", objectResult.Value);
        }

        [Fact]
        public async Task DeleteRoom_ReturnsOkResult_WhenRoomIsDeleted()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Succeeded = true,
                Message = "Room deleted successfully."
            };

            _roomServiceMock
                .Setup(service => service.DeleteAsync(roomId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteRoom(roomId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<bool>>(okResult.Value); // Updated to match Response<bool>
            Assert.True(response.Succeeded);
            Assert.Equal("Room deleted successfully.", response.Message);
        }


        [Fact]
        public async Task DeleteRoom_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var roomId = Guid.NewGuid();

            _roomServiceMock.Setup(service => service.DeleteAsync(roomId))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.DeleteRoom(roomId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var responseData = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("Internal server error: Internal server error", responseData.Message);
        }

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
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Room not found.", response.Message);
        }

        [Fact]
        public async Task GetRoomsByHostelId_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            int? floor = 2;

            _roomServiceMock.Setup(service => service.GetRoomsByHostelIdAsync(hostelId, floor))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.GetRoomsByHostelId(hostelId, floor);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var responseData = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("Internal server error: Internal server error", responseData.Message);
        }

        [Fact]
        public async Task GetRoomsByHostelId_ReturnsBadRequest_WhenRoomsDoNotExist()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            int? floor = 2;
            var mockResponse = new Response<List<RoomResponseDto>>
            {
                Succeeded = false,
                Message = "Không tìm thấy phòng nào trong trọ"
            };

            _roomServiceMock.Setup(service => service.GetRoomsByHostelIdAsync(hostelId, floor))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetRoomsByHostelId(hostelId, floor);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseData = Assert.IsType<Response<List<RoomResponseDto>>>(badRequestResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("Không tìm thấy phòng nào trong trọ", responseData.Message);
        }

        [Fact]
        public async Task GetRoomsByHostelId_ReturnsOkResult_WhenRoomsExist()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            int? floor = 2;
            var mockRooms = new List<RoomResponseDto>
    {
        new RoomResponseDto { Id = Guid.NewGuid(), RoomName = "Room A", MonthlyRentCost = 1000, ImageRoom = "http://example.com/roomA.jpg" },
        new RoomResponseDto { Id = Guid.NewGuid(), RoomName = "Room B", MonthlyRentCost = 1500, ImageRoom = "http://example.com/roomB.jpg" }
    };

            var mockResponse = new Response<List<RoomResponseDto>>(mockRooms);

            _roomServiceMock.Setup(service => service.GetRoomsByHostelIdAsync(hostelId, floor))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetRoomsByHostelId(hostelId, floor);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<List<RoomResponseDto>>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal(2, responseData.Data.Count);
            Assert.Equal("Room A", responseData.Data[0].RoomName);
            Assert.Equal("http://example.com/roomA.jpg", responseData.Data[0].ImageRoom);
        }

    }
}
