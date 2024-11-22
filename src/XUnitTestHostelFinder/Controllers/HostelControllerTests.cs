using HostelFinder.Application.DTOs.Address;
using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.DTOs.Image.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Common.Constants;
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
            var mockHostel = new HostelResponseDto
            {
                Id = hostelId,
                HostelName = "Test Hostel",
                Description = "A lovely hostel",
                Address = new AddressDto
                {
                    Province = "123 Test Street",
                    District = "Test City",
                    Commune = "Test Province",
                    DetailAddress = "12345"
                },
                Size = 1000,
                NumberOfRooms = 20,
                Image = new List<ImageResponseDto>
        {
            new ImageResponseDto { Url = "https://example.com/image1.jpg" },
            new ImageResponseDto { Url = "https://example.com/image2.jpg" }
        },
                Coordinates = "10.123,20.456",
                CreatedOn = DateTimeOffset.UtcNow
            };

            var response = new Response<HostelResponseDto>(mockHostel);

            _hostelServiceMock.Setup(service => service.GetHostelByIdAsync(hostelId))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetHostelById(hostelId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<HostelResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.NotNull(returnValue.Data);
            Assert.Equal("Test Hostel", returnValue.Data.HostelName);
            Assert.Equal("A lovely hostel", returnValue.Data.Description);
            Assert.Equal("123 Test Street", returnValue.Data.Address.Province);
            Assert.Equal(1000, returnValue.Data.Size);
            Assert.Equal(20, returnValue.Data.NumberOfRooms);
            Assert.NotEmpty(returnValue.Data.Image);
            Assert.Equal("https://example.com/image1.jpg", returnValue.Data.Image.First().Url);
            Assert.Equal("10.123,20.456", returnValue.Data.Coordinates);
        }

        [Fact]
        public async Task GetHostelById_ReturnsNotFound_WhenHostelDoesNotExist()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var response = new Response<HostelResponseDto>("Hostel not found.")
            {
                Succeeded = false
            };

            _hostelServiceMock.Setup(service => service.GetHostelByIdAsync(hostelId))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetHostelById(hostelId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Response<HostelResponseDto>>(notFoundResult.Value);
            Assert.False(returnValue.Succeeded);
            Assert.Equal("Hostel not found.", returnValue.Message);
        }

        [Fact]
        public async Task GetHostelById_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            _hostelServiceMock.Setup(service => service.GetHostelByIdAsync(hostelId))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.GetHostelById(hostelId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var returnValue = Assert.IsType<Response<HostelResponseDto>>(objectResult.Value);
            Assert.False(returnValue.Succeeded);
            Assert.Equal("Internal server error", returnValue.Message);
        }
        [Fact]
        public async Task AddHostel_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var addHostelRequest = new AddHostelRequestDto
            {
                HostelName = "Test Hostel",
                Description = "Test Description",
                Address = new AddressDto
                {
                    Province = "Test Province",
                    District = "Test District",
                    Commune = "Test Commune",
                    DetailAddress = "123 Test Street"
                },
                ServiceId = new List<Guid> { Guid.NewGuid() }.Select(id => (Guid?)id).ToList(),
                LandlordId = Guid.NewGuid()
            };
            var images = new List<IFormFile>();
            var response = new Response<HostelResponseDto>
            {
                Succeeded = true,
                Data = new HostelResponseDto { Id = Guid.NewGuid(), HostelName = "Test Hostel" },
                Message = "Thêm trọ mới thành công."
            };

            _hostelServiceMock.Setup(s => s.AddHostelAsync(addHostelRequest, It.IsAny<List<string>>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.AddHostel(addHostelRequest, images);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Expecting OkObjectResult
            var responseData = Assert.IsType<Response<HostelResponseDto>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal("Test Hostel", responseData.Data.HostelName);
            Assert.Equal("Thêm trọ mới thành công.", responseData.Message);
            Assert.NotNull(responseData.Data.Id); // Ensure ID is present
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
            var updateHostelDto = new UpdateHostelRequestDto
            {
                HostelName = "Updated Hostel Name",
                Description = "Updated Description",
                Address = new AddressDto
                {
                    Province = "123 Updated Street",
                    District = "Updated City",
                    Commune = "Updated Province",
                    DetailAddress = "12345"
                },
                Size = 2000,
                NumberOfRooms = 25,
                Coordinates = "10.123,20.456",
                ServiceId = new List<Guid?> { Guid.NewGuid() }
            };

            var mockResponse = new Response<HostelResponseDto>(
                new HostelResponseDto
                {
                    Id = hostelId,
                    HostelName = "Updated Hostel Name",
                    Description = "Updated Description",
                    Address = updateHostelDto.Address,
                    Size = 2000,
                    NumberOfRooms = 25,
                    Coordinates = "10.123,20.456",
                    CreatedOn = DateTime.UtcNow
                },
                "Hostel updated successfully"
            );

            _hostelServiceMock.Setup(service => service.UpdateHostelAsync(hostelId, updateHostelDto, It.IsAny<List<string>>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateHostel(hostelId, updateHostelDto, new List<IFormFile>());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<HostelResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.Equal("Hostel updated successfully", returnValue.Message);
            Assert.Equal("Updated Hostel Name", returnValue.Data.HostelName);
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
        public async Task DeleteHostel_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var hostelId = Guid.NewGuid();

            _hostelServiceMock.Setup(service => service.DeleteHostelAsync(hostelId))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.DeleteHostel(hostelId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var responseData = Assert.IsType<Response<bool>>(objectResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("Internal server error: Internal server error", responseData.Message);
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

            if (expectedSuccess)
            {
                var okResult = Assert.IsType<OkObjectResult>(result); // Changed from CreatedAtActionResult
                var responseData = Assert.IsType<Response<HostelResponseDto>>(okResult.Value);
                Assert.True(responseData.Succeeded);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequestResult.Value); // Ensure validation error returned
            }
        }

        [Fact]
        public async Task UpdateHostel_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("HostelName", "HostelName is required");

            // Act
            var result = await _controller.UpdateHostel(Guid.NewGuid(), null, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<HostelResponseDto>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Invalid model state.", response.Message);
        }

        [Fact]
        public async Task UpdateHostel_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var updateHostelDto = new UpdateHostelRequestDto
            {
                HostelName = "Updated Hostel Name",
                Description = "Updated Description",
                Address = new AddressDto
                {
                    Province = "123 Updated Street",
                    District = "Updated City",
                    Commune = "Updated Province",
                    DetailAddress = "12345"
                },
                Size = 2000,
                NumberOfRooms = 25,
                Coordinates = "10.123,20.456",
                ServiceId = new List<Guid?> { Guid.NewGuid() }
            };

            var mockResponse = new Response<HostelResponseDto>(null, "Update failed")
            {
                Succeeded = false
            };

            _hostelServiceMock.Setup(service => service.UpdateHostelAsync(hostelId, updateHostelDto, It.IsAny<List<string>>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateHostel(hostelId, updateHostelDto, new List<IFormFile>());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<HostelResponseDto>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Update failed", response.Message);
        }


        [Fact]
        public async Task UpdateHostel_ReturnsNotFound_WhenHostelDoesNotExist()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var updateHostelDto = new UpdateHostelRequestDto
            {
                HostelName = "Updated Hostel Name",
                Description = "Updated Description",
                Address = new AddressDto
                {
                    Province = "123 Updated Street",
                    District = "Updated City",
                    Commune = "Updated Province",
                    DetailAddress = "12345"
                },
                Size = 2000,
                NumberOfRooms = 25,
                Coordinates = "10.123,20.456",
                ServiceId = new List<Guid?> { Guid.NewGuid() }
            };

            var mockResponse = new Response<HostelResponseDto>("Hostel not found.");

            _hostelServiceMock.Setup(service => service.UpdateHostelAsync(hostelId, updateHostelDto, It.IsAny<List<string>>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateHostel(hostelId, updateHostelDto, new List<IFormFile>());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<HostelResponseDto>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Hostel not found.", response.Message);
        }

        [Fact]
        public async Task UpdateHostel_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var hostelId = Guid.NewGuid();
            var updateHostelDto = new UpdateHostelRequestDto
            {
                HostelName = "Updated Hostel Name",
                Description = "Updated Description",
                Address = new AddressDto
                {
                    Province = "123 Updated Street",
                    District = "Updated City",
                    Commune = "Updated Province",
                    DetailAddress = "12345"
                },
                Size = 2000,
                NumberOfRooms = 25,
                Coordinates = "10.123,20.456",
                ServiceId = new List<Guid?> { Guid.NewGuid() }
            };

            _hostelServiceMock.Setup(service => service.UpdateHostelAsync(hostelId, updateHostelDto, It.IsAny<List<string>>()))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.UpdateHostel(hostelId, updateHostelDto, new List<IFormFile>());

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var response = Assert.IsType<Response<HostelResponseDto>>(objectResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Internal server error: Internal server error", response.Message);
        }

        [Fact]
        public async Task GetHostelsByLandlordId_ReturnsOkResult_WhenHostelsExist()
        {
            // Arrange
            var landlordId = Guid.NewGuid();
            var hostels = new List<ListHostelResponseDto>
    {
        new ListHostelResponseDto { Id = Guid.NewGuid(), HostelName = "Hostel 1" },
        new ListHostelResponseDto { Id = Guid.NewGuid(), HostelName = "Hostel 2" }
    };

            var response = new Response<List<ListHostelResponseDto>>(hostels);

            _hostelServiceMock.Setup(service => service.GetHostelsByUserIdAsync(landlordId))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetHostelsByLandlordId(landlordId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<List<ListHostelResponseDto>>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal(2, responseData.Data.Count);
        }

        [Fact]
        public async Task GetHostelsByLandlordId_ReturnsNotFound_WhenNoHostelsExist()
        {
            // Arrange
            var landlordId = Guid.NewGuid();
            var response = new Response<List<ListHostelResponseDto>>(null)
            {
                Succeeded = false,
                Errors = new List<string> { "No hostels found" }
            };

            _hostelServiceMock.Setup(service => service.GetHostelsByUserIdAsync(landlordId))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetHostelsByLandlordId(landlordId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errors = Assert.IsType<List<string>>(notFoundResult.Value);
            Assert.Contains("No hostels found", errors);
        }

        [Fact]
        public async Task GetHostelsByLandlordId_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var landlordId = Guid.NewGuid();

            _hostelServiceMock.Setup(service => service.GetHostelsByUserIdAsync(landlordId))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.GetHostelsByLandlordId(landlordId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WhenHostelsExist()
        {
            // Arrange
            var query = new GetAllHostelQuery
            {
                SearchPhrase = "Hostel",
                PageSize = 2,
                PageNumber = 1,
                SortBy = "HostelName",
                SortDirection = SortDirection.Ascending
            };

            var hostels = new List<ListHostelResponseDto>
    {
        new ListHostelResponseDto { Id = Guid.NewGuid(), HostelName = "Hostel A" },
        new ListHostelResponseDto { Id = Guid.NewGuid(), HostelName = "Hostel B" }
    };

            var response = new PagedResponse<List<ListHostelResponseDto>>(hostels, query.PageNumber, query.PageSize)
            {
                TotalRecords = 5,
                TotalPages = 3,
                NextPage = new Uri("http://example.com/api/hostels?page=2"),
                PreviousPage = null // Since this is the first page
            };

            _hostelServiceMock.Setup(service => service.GetAllHostelAsync(query))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<PagedResponse<List<ListHostelResponseDto>>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal(2, responseData.Data.Count);
            Assert.Equal(5, responseData.TotalRecords);
            Assert.Equal(3, responseData.TotalPages);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoHostelsMatch()
        {
            // Arrange
            var query = new GetAllHostelQuery
            {
                SearchPhrase = "NonExistentHostel",
                PageSize = 2,
                PageNumber = 1,
                SortBy = "HostelName",
                SortDirection = SortDirection.Descending
            };

            var response = new PagedResponse<List<ListHostelResponseDto>>(null, query.PageNumber, query.PageSize)
            {
                Succeeded = false,
                Message = "No hostels found."
            };

            _hostelServiceMock.Setup(service => service.GetAllHostelAsync(query))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var responseData = Assert.IsType<PagedResponse<List<ListHostelResponseDto>>>(notFoundResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("No hostels found.", responseData.Message);
        }

        [Fact]
        public async Task GetAll_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var query = new GetAllHostelQuery
            {
                SearchPhrase = "Hostel",
                PageSize = 2,
                PageNumber = 1,
                SortBy = "HostelName",
                SortDirection = SortDirection.Ascending
            };

            _hostelServiceMock.Setup(service => service.GetAllHostelAsync(query))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }

        [Theory]
        [InlineData(SortDirection.Ascending, "Ascending")]
        [InlineData(SortDirection.Descending, "Descending")]
        public async Task GetAll_HandlesSortDirectionParameter(SortDirection sortDirection, string sortDirectionText)
        {
            // Arrange
            var query = new GetAllHostelQuery
            {
                SearchPhrase = "Hostel",
                PageSize = 2,
                PageNumber = 1,
                SortBy = "HostelName",
                SortDirection = sortDirection
            };

            var hostels = new List<ListHostelResponseDto>
    {
        new ListHostelResponseDto { Id = Guid.NewGuid(), HostelName = $"Hostel {sortDirectionText}" }
    };

            // Use the three-argument constructor and then set additional properties
            var response = new PagedResponse<List<ListHostelResponseDto>>(hostels, query.PageNumber, query.PageSize)
            {
                TotalRecords = 1, // Total number of records
                TotalPages = 1 // Since there's only 1 record
            };

            _hostelServiceMock.Setup(service => service.GetAllHostelAsync(query))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<PagedResponse<List<ListHostelResponseDto>>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Single(responseData.Data);
            Assert.Equal($"Hostel {sortDirectionText}", responseData.Data.First().HostelName);
            Assert.Equal(1, responseData.TotalRecords);
            Assert.Equal(1, responseData.TotalPages);
        }


    }
}
