using HostelFinder.Application.DTOs.Address;
using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestHostelFinder.Controllers
{
    public class HostelControllerTests
    {
        private readonly Mock<IHostelService> _hostelServiceMock;
        private readonly HostelController _controller;

        public HostelControllerTests()
        {
            _hostelServiceMock = new Mock<IHostelService>();
            _controller = new HostelController(_hostelServiceMock.Object);
        }

        [Fact]
        public async Task GetHostelById_ReturnsOkResult_WhenHostelExists()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var mockHostelResponse = new Response<HostelResponseDto>
            {
                Data = new HostelResponseDto
                {
                    Id = hostelId,
                    HostelName = "Mock Hostel",
                    Description = "A nice place to stay.",
                    Address = new AddressDto
                    {
                        Province = "Test Province",
                        District = "Test District",
                        Commune = "Test Commune",
                        DetailAddress = "123 Test Street"
                    },
                    NumberOfRooms = 10,
                    Rating = 4.5f,
                    Image = "image_url",
                    Coordinates = "10.762622,106.660172",
                    CreatedOn = DateTimeOffset.Now
                },
                Succeeded = true
            };

            _hostelServiceMock
                .Setup(service => service.GetHostelByIdAsync(hostelId))
                .ReturnsAsync(mockHostelResponse);

            // Act
            var result = await _controller.GetHostelById(hostelId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<HostelResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.Equal("Mock Hostel", returnValue.Data.HostelName);
        }

        [Fact]
        public async Task GetHostelById_ReturnsNotFound_WhenHostelDoesNotExist()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var mockHostelResponse = new Response<HostelResponseDto>
            {
                Data = null,
                Succeeded = false,
                Errors = new List<string> { "Hostel not found" }
            };

            _hostelServiceMock
                .Setup(service => service.GetHostelByIdAsync(hostelId))
                .ReturnsAsync(mockHostelResponse);

            // Act
            var result = await _controller.GetHostelById(hostelId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Response<HostelResponseDto>>(notFoundResult.Value);
            Assert.False(returnValue.Succeeded);
            Assert.Contains("Hostel not found", returnValue.Errors);
        }

        [Fact]
        public async Task GetHostelsByLandlordId_ReturnsOkResult_WhenHostelsExist()
        {
            // Arrange
            var landlordId = Guid.NewGuid();
            var mockHostels = new List<HostelResponseDto>
            {
                new HostelResponseDto { Id = Guid.NewGuid(), HostelName = "Hostel 1" },
                new HostelResponseDto { Id = Guid.NewGuid(), HostelName = "Hostel 2" }
            };

            var mockHostelsResponse = new Response<List<HostelResponseDto>>
            {
                Data = mockHostels,
                Succeeded = true
            };

            _hostelServiceMock
                .Setup(service => service.GetHostelsByLandlordIdAsync(landlordId))
                .ReturnsAsync(mockHostelsResponse);

            // Act
            var result = await _controller.GetHostelsByLandlordId(landlordId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<List<HostelResponseDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.Equal(2, returnValue.Data.Count);
        }


        [Fact]
        public async Task GetHostelsByLandlordId_ReturnsOkResult_WhenNoHostelsFound()
        {
            // Arrange
            var landlordId = Guid.NewGuid();
            var mockHostelsResponse = new Response<List<HostelResponseDto>>
            {
                Data = new List<HostelResponseDto>(),
                Succeeded = true
            };

            // Setting up the mock to return the expected response
            _hostelServiceMock
                .Setup(service => service.GetHostelsByLandlordIdAsync(landlordId))
                .ReturnsAsync(mockHostelsResponse);

            // Act
            var result = await _controller.GetHostelsByLandlordId(landlordId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<List<HostelResponseDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.Empty(returnValue.Data);
        }


        [Fact]
        public async Task AddHostel_ReturnsOkResult_WhenAddSucceeds()
        {
            // Arrange
            var hostelDto = new AddHostelRequestDto
            {
                LandlordId = Guid.NewGuid(), 
                HostelName = "New Hostel",
                Description = "A description of the hostel.",
                Address = new AddressDto
                {
                    Province = "Some Province",
                    District = "Some District",
                    Commune = "Some Commune",
                    DetailAddress = "Some Detail Address"
                },
                Size = 100.0f,
                NumberOfRooms = 10,
                Coordinates = "Some Coordinates",
                Rating = 4.5f
            };

            var mockHostelResponse = new HostelResponseDto
            {
                Id = Guid.NewGuid(), 
                HostelName = hostelDto.HostelName,
            };

            var mockResponse = new Response<HostelResponseDto>
            {
                Data = mockHostelResponse,
                Succeeded = true
            };

            // Ensure the setup is using the correct method signature
            _hostelServiceMock
                .Setup(service => service.AddHostelAsync(It.IsAny<AddHostelRequestDto>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddHostel(hostelDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<HostelResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.Equal(mockHostelResponse.HostelName, returnValue.Data.HostelName);
        }


        [Fact]
        public async Task AddHostel_ReturnsBadRequest_WhenAddFails()
        {
            // Arrange
            var hostelDto = new AddHostelRequestDto
            {
                HostelName = "New Hostel",
                LandlordId = Guid.NewGuid(), 
                Description = "A description of the hostel.",
                Address = new AddressDto
                {
                    Province = "Some Province",
                    District = "Some District",
                    Commune = "Some Commune",
                    DetailAddress = "Some Detail Address"
                },
                Size = 100.0f,
                NumberOfRooms = 10,
                Coordinates = "Some Coordinates",
                Rating = 4.5f
            };

            var mockResponse = new Response<HostelResponseDto>
            {
                Data = null,
                Succeeded = false,
                Errors = new List<string> { "Add failed" }
            };

            // Ensure the setup is using the correct method signature
            _hostelServiceMock
                .Setup(service => service.AddHostelAsync(It.IsAny<AddHostelRequestDto>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddHostel(hostelDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<HostelResponseDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
            Assert.Contains("Add failed", returnValue.Errors);
        }

        [Fact]
        public async Task UpdateHostel_ReturnsOkResult_WhenUpdateSucceeds()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var hostelDto = new UpdateHostelRequestDto
            {
                HostelName = "Updated Hostel",
                Description = "A cozy hostel in the city center",
                Address = new AddressDto
                {
                    Province = "Hanoi",
                    District = "Hoan Kiem",
                    Commune = "Cau Go",
                    DetailAddress = "123 Street Name"
                },
                Size = 150.5f,
                NumberOfRooms = 10,
                Coordinates = "21.0285, 105.8542",
                Rating = 4.5f
            };

            var mockResponse = new Response<HostelResponseDto>
            {
                Data = new HostelResponseDto
                {
                    Id = hostelId,
                    HostelName = "Updated Hostel",
                    Description = "A cozy hostel in the city center",
                    Address = hostelDto.Address,
                    NumberOfRooms = hostelDto.NumberOfRooms,
                    Coordinates = hostelDto.Coordinates,
                    Rating = hostelDto.Rating
                },
                Succeeded = true
            };

            _hostelServiceMock
                .Setup(service => service.UpdateHostelAsync(hostelDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateHostel(hostelId, hostelDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<HostelResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.Equal("Updated Hostel", returnValue.Data.HostelName);
            Assert.Equal("A cozy hostel in the city center", returnValue.Data.Description);
            Assert.Equal(10, returnValue.Data.NumberOfRooms);
            Assert.Equal("21.0285, 105.8542", returnValue.Data.Coordinates);
            Assert.Equal(4.5f, returnValue.Data.Rating);
        }

        [Fact]
        public async Task UpdateHostel_ReturnsNotFound_WhenUpdateFails()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var hostelDto = new UpdateHostelRequestDto
            {
                HostelName = "Updated Hostel",
                Description = "A cozy hostel in the city center",
                Address = new AddressDto
                {
                    Province = "Hanoi",
                    District = "Hoan Kiem",
                    Commune = "Cau Go",
                    DetailAddress = "123 Street Name"
                },
                Size = 150.5f,
                NumberOfRooms = 10,
                Coordinates = "21.0285, 105.8542",
                Rating = 4.5f
            };

            var mockResponse = new Response<HostelResponseDto>
            {
                Data = null,
                Succeeded = false,
                Errors = new List<string> { "Hostel not found" }
            };

            _hostelServiceMock
                .Setup(service => service.UpdateHostelAsync(hostelDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateHostel(hostelId, hostelDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Response<HostelResponseDto>>(notFoundResult.Value);
            Assert.False(returnValue.Succeeded);
            Assert.Contains("Hostel not found", returnValue.Errors);
        }

        [Fact]
        public async Task DeleteHostel_ReturnsOkResult_WhenDeleteSucceeds()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Data = true,
                Succeeded = true
            };

            // Set up the mock to return the success response
            _hostelServiceMock
                .Setup(service => service.DeleteHostelAsync(hostelId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteHostel(hostelId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.True(returnValue.Data); // Data should be true indicating deletion succeeded
        }

        [Fact]
        public async Task DeleteHostel_ReturnsNotFound_WhenDeleteFails()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Data = false, // Indicating failure
                Succeeded = false,
                Errors = new List<string> { "Hostel not found" }
            };

            // Set up the mock to return the failure response
            _hostelServiceMock
                .Setup(service => service.DeleteHostelAsync(hostelId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteHostel(hostelId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(notFoundResult.Value);
            Assert.False(returnValue.Succeeded);
            Assert.Contains("Hostel not found", returnValue.Errors);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WhenGetAllSucceeds()
        {
            // Arrange
            var request = new GetAllHostelQuery(); // You can set properties if needed
            var mockResponse = new PagedResponse<List<ListHostelResponseDto>>
            {
                Data = new List<ListHostelResponseDto>
        {
            new ListHostelResponseDto { HostelName = "Hostel A", Rating = 4.5f },
            new ListHostelResponseDto { HostelName = "Hostel B", Rating = 4.0f }
        },
                Succeeded = true
            };

            // Set up the mock to return the success response
            _hostelServiceMock
                .Setup(service => service.GetAllHostelAsync(request))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<ListHostelResponseDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.NotNull(returnValue.Data);
            Assert.Equal(2, returnValue.Data.Count); // Adjust this count based on mock data
        }

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenGetAllFails()
        {
            // Arrange
            var request = new GetAllHostelQuery(); // You can set properties if needed
            var mockResponse = new PagedResponse<List<ListHostelResponseDto>>
            {
                Data = null,
                Succeeded = false,
                Errors = new List<string> { "No hostels found" }
            };

            // Set up the mock to return the failure response
            _hostelServiceMock
                .Setup(service => service.GetAllHostelAsync(request))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<ListHostelResponseDto>>>(notFoundResult.Value);
            Assert.False(returnValue.Succeeded);
            Assert.Contains("No hostels found", returnValue.Errors);
        }


    }
}
