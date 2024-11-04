using HostelFinder.Application.DTOs.ServiceCost.Request;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using HostelFinder.WebApi.Controllers;
using HostelFinder.Application.Wrappers;

namespace XUnitTestHostelFinder.Controllers
{
    public class ServiceCostControllerTests
    {
        private readonly Mock<IServiceCostService> _serviceCostServiceMock;
        private readonly ServiceCostController _controller;

        public ServiceCostControllerTests()
        {
            _serviceCostServiceMock = new Mock<IServiceCostService>();
            _controller = new ServiceCostController(_serviceCostServiceMock.Object);
        }

        [Fact]
        public async Task GetServiceCosts_ReturnsOkResult_WhenServiceCostsExist()
        {
            // Arrange
            var mockResponse = new Response<List<ServiceCostResponseDto>>
            {
                Data = new List<ServiceCostResponseDto>
                {
                    new ServiceCostResponseDto { Id = Guid.NewGuid(), ServiceName = "Service 1" },
                    new ServiceCostResponseDto { Id = Guid.NewGuid(), ServiceName = "Service 2" }
                },
                Succeeded = true
            };

            _serviceCostServiceMock
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetServiceCosts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ServiceCostResponseDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetServiceCost_ReturnsOkResult_WhenServiceCostExists()
        {
            // Arrange
            var serviceCostId = Guid.NewGuid();
            var mockResponse = new Response<ServiceCostResponseDto>
            {
                Data = new ServiceCostResponseDto { Id = serviceCostId, ServiceName = "Test Service" },
                Succeeded = true
            };

            _serviceCostServiceMock
                .Setup(service => service.GetByIdAsync(serviceCostId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetServiceCost(serviceCostId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ServiceCostResponseDto>(okResult.Value);
            Assert.Equal("Test Service", returnValue.ServiceName);
        }

        [Fact]
        public async Task GetServiceCost_ReturnsNotFound_WhenServiceCostDoesNotExist()
        {
            // Arrange
            var serviceCostId = Guid.NewGuid();
            var mockResponse = new Response<ServiceCostResponseDto>
            {
                Data = null,
                Succeeded = false,
                Message = "Service cost not found."
            };

            _serviceCostServiceMock
                .Setup(service => service.GetByIdAsync(serviceCostId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetServiceCost(serviceCostId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Service cost not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateServiceCost_ReturnsOkResult_WhenCreationSucceeds()
        {
            // Arrange
            var serviceCostDto = new AddServiceCostDto
            {
                ServiceName = "New Service",
                UnitCost = 100,
                Cost = 200
            };

            var mockResponse = new Response<ServiceCostResponseDto>
            {
                Data = new ServiceCostResponseDto { Id = Guid.NewGuid(), ServiceName = "New Service" },
                Succeeded = true
            };

            _serviceCostServiceMock
                .Setup(service => service.CreateAsync(It.IsAny<AddServiceCostDto>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CreateServiceCost(serviceCostDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ServiceCostResponseDto>(okResult.Value);
            Assert.Equal("New Service", returnValue.ServiceName);
        }

        [Fact]
        public async Task CreateServiceCost_ReturnsBadRequest_WhenCreationFails()
        {
            // Arrange
            var serviceCostDto = new AddServiceCostDto
            {
                ServiceName = "New Service",
                UnitCost = 100,
                Cost = 200
            };

            var mockResponse = new Response<ServiceCostResponseDto>
            {
                Data = null,
                Succeeded = false,
                Message = "Service cost creation failed."
            };

            _serviceCostServiceMock
                .Setup(service => service.CreateAsync(It.IsAny<AddServiceCostDto>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CreateServiceCost(serviceCostDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Service cost creation failed.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateServiceCost_ReturnsOkResult_WhenUpdateSucceeds()
        {
            // Arrange
            var serviceCostId = Guid.NewGuid();
            var serviceCostDto = new UpdateServiceCostDto
            {
                ServiceName = "Updated Service",
                UnitCost = 150,
                Cost = 300
            };

            var mockResponse = new Response<ServiceCostResponseDto>
            {
                Data = new ServiceCostResponseDto { Id = serviceCostId, ServiceName = "Updated Service" },
                Succeeded = true
            };

            _serviceCostServiceMock
                .Setup(service => service.UpdateAsync(serviceCostId, It.IsAny<UpdateServiceCostDto>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateServiceCost(serviceCostId, serviceCostDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ServiceCostResponseDto>(okResult.Value);
            Assert.Equal("Updated Service", returnValue.ServiceName);
        }

        [Fact]
        public async Task UpdateServiceCost_ReturnsNotFound_WhenServiceCostDoesNotExist()
        {
            // Arrange
            var serviceCostId = Guid.NewGuid();
            var serviceCostDto = new UpdateServiceCostDto
            {
                ServiceName = "Updated Service",
                UnitCost = 150,
                Cost = 300
            };

            var mockResponse = new Response<ServiceCostResponseDto>
            {
                Data = null,
                Succeeded = false,
                Message = "Service cost not found."
            };

            _serviceCostServiceMock
                .Setup(service => service.UpdateAsync(serviceCostId, It.IsAny<UpdateServiceCostDto>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateServiceCost(serviceCostId, serviceCostDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Service cost not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteServiceCost_ReturnsOkResult_WhenDeletionSucceeds()
        {
            // Arrange
            var serviceCostId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Data = true,
                Succeeded = true
            };

            _serviceCostServiceMock
                .Setup(service => service.DeleteAsync(serviceCostId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteServiceCost(serviceCostId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(returnValue.Data);
        }

        [Fact]
        public async Task DeleteServiceCost_ReturnsNotFound_WhenServiceCostDoesNotExist()
        {
            // Arrange
            var serviceCostId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Data = false,
                Succeeded = false,
                Message = "Service cost not found."
            };

            _serviceCostServiceMock
                .Setup(service => service.DeleteAsync(serviceCostId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteServiceCost(serviceCostId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Service cost not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateServiceCost_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var invalidServiceCostDto = new AddServiceCostDto(); // Missing required fields like ServiceName, Cost

            _controller.ModelState.AddModelError("ServiceName", "Required");

            // Act
            var result = await _controller.CreateServiceCost(invalidServiceCostDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task CreateServiceCost_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            var serviceCostDto = new AddServiceCostDto
            {
                ServiceName = "Test Service",
                Cost = 200
            };

            _serviceCostServiceMock
                .Setup(service => service.CreateAsync(It.IsAny<AddServiceCostDto>()))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.CreateServiceCost(serviceCostDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }

        [Theory]
        [InlineData("Service 1", 200, 10, true)] // Valid input
        [InlineData("Service 2", -50, 5, false)] // Invalid cost
        [InlineData("", 150, 3, false)] // Missing service name
        public async Task CreateServiceCost_WithDifferentInputs_ReturnsExpectedResult(string serviceName, decimal cost, int currentReading, bool expectedSuccess)
        {
            // Arrange
            var serviceCostDto = new AddServiceCostDto
            {
                ServiceName = serviceName,
                Cost = cost,
                CurrentReading = currentReading
            };

            Response<ServiceCostResponseDto> mockResponse;
            if (expectedSuccess)
            {
                mockResponse = new Response<ServiceCostResponseDto>
                {
                    Data = new ServiceCostResponseDto { ServiceName = serviceName },
                    Succeeded = true
                };
            }
            else
            {
                mockResponse = new Response<ServiceCostResponseDto>
                {
                    Data = null,
                    Succeeded = false,
                    Message = "Invalid service cost data."
                };
            }

            _serviceCostServiceMock
                .Setup(service => service.CreateAsync(It.IsAny<AddServiceCostDto>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CreateServiceCost(serviceCostDto);

            // Assert
            if (expectedSuccess)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<ServiceCostResponseDto>(okResult.Value);
                Assert.Equal(serviceName, returnValue.ServiceName);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid service cost data.", badRequestResult.Value);
            }
        }

    }
}
