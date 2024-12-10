using HostelFinder.Application.DTOs.Service.Response;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace XUnitTestHostelFinder.Controllers
{
    public class ServiceControllerTests
    {
        private readonly Mock<IServiceService> _serviceServiceMock;
        private readonly ServiceController _controller;

        public ServiceControllerTests()
        {
            _serviceServiceMock = new Mock<IServiceService>();
            _controller = new ServiceController(_serviceServiceMock.Object);
        }

        [Fact]
        public async Task GetAllServices_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _serviceServiceMock.Setup(service => service.GetAllServicesAsync())
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllServices();

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);

            var response = Assert.IsType<Response<string>>(internalServerErrorResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Internal server error: Unexpected error", response.Message);
        }

        [Fact]
        public async Task GetAllServices_ReturnsNotFound_WhenNoServicesExist()
        {
            // Arrange
            var mockResponse = new Response<List<ServiceResponseDTO>>
            {
                Succeeded = true,
                Data = new List<ServiceResponseDTO>()
            };

            _serviceServiceMock.Setup(service => service.GetAllServicesAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAllServices();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("No services available.", response.Message);
        }

        [Fact]
        public async Task GetAllServices_ReturnsOkResult_WhenServicesExist()
        {
            // Arrange
            var mockServices = new List<ServiceResponseDTO>
    {
        new ServiceResponseDTO { Id = Guid.NewGuid(), ServiceName = "Service 1" },
        new ServiceResponseDTO { Id = Guid.NewGuid(), ServiceName = "Service 2" }
    };

            var mockResponse = new Response<List<ServiceResponseDTO>>
            {
                Succeeded = true,
                Data = mockServices
            };

            _serviceServiceMock.Setup(service => service.GetAllServicesAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAllServices();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<List<ServiceResponseDTO>>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.NotEmpty(response.Data);
            Assert.Equal(2, response.Data.Count);
        }

    }
}
