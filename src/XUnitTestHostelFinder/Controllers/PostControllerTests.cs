using HostelFinder.Application.DTOs.Address;
using HostelFinder.Application.DTOs.Post.Requests;
using HostelFinder.Application.DTOs.Post.Responses;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Helpers;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace XUnitTestHostelFinder.Controllers
{
    public class PostControllerTests
    {
        private readonly PostController _controller;
        private readonly Mock<IPostService> _postServiceMock;
        private readonly Mock<IS3Service> _s3ServiceMock;
        private readonly Mock<IOpenAiService> _openAiService;

        public PostControllerTests()
        {
            _postServiceMock = new Mock<IPostService>();
            _s3ServiceMock = new Mock<IS3Service>();
            _controller = new PostController(_postServiceMock.Object, _s3ServiceMock.Object, _openAiService.Object);
        }

        [Fact]
        public async Task GetAllPostWithPriceAndStatusAndTime_ReturnsCorrectPostData()
        {
            // Arrange
            var mockPosts = new List<ListPostsResponseDto>
    {
        new ListPostsResponseDto
        {
            Id = Guid.NewGuid(),
            Title = "Post A",
            Address = new AddressDto { Province = "Province A", District = "District A", Commune = "Commune A" },
            MonthlyRentCost = 1200,
            Size = 35,
            FirstImage = "image_url_1.jpg",
            CreatedOn = DateTimeOffset.UtcNow.AddDays(-1)
        },
        new ListPostsResponseDto
        {
            Id = Guid.NewGuid(),
            Title = "Post B",
            Address = new AddressDto { Province = "Province B", District = "District B", Commune = "Commune B" },
            MonthlyRentCost = 1500,
            Size = 40,
            FirstImage = "image_url_2.jpg",
            CreatedOn = DateTimeOffset.UtcNow.AddDays(-2)
        }
    };

            var mockResponse = new Response<List<ListPostsResponseDto>>
            {
                Data = mockPosts,
                Succeeded = true
            };

            _postServiceMock.Setup(service => service.GetAllPostWithPriceAndStatusAndTime())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAllPostWithPriceAndStatusAndTime();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<List<ListPostsResponseDto>>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal(2, responseData.Data.Count);
            Assert.Equal("Post A", responseData.Data.First().Title);
            Assert.Equal("Province A", responseData.Data.First().Address.Province);
        }

        [Fact]
        public async Task GetAllPostWithPriceAndStatusAndTime_ReturnsPartialData_WhenFieldsAreMissing()
        {
            // Arrange
            var mockPosts = new List<ListPostsResponseDto>
    {
        new ListPostsResponseDto
        {
            Id = Guid.NewGuid(),
            Title = "Post Without Address",
            Address = null, // Simulating missing Address
            MonthlyRentCost = 0, // Simulating missing cost
            Size = 0, // Simulating missing size
            FirstImage = null, // Simulating missing image
            CreatedOn = DateTimeOffset.UtcNow.AddDays(-3)
        }
    };

            var mockResponse = new Response<List<ListPostsResponseDto>>
            {
                Data = mockPosts,
                Succeeded = true
            };

            _postServiceMock.Setup(service => service.GetAllPostWithPriceAndStatusAndTime())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAllPostWithPriceAndStatusAndTime();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<List<ListPostsResponseDto>>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Single(responseData.Data);
            Assert.Equal("Post Without Address", responseData.Data.First().Title);
            Assert.Null(responseData.Data.First().Address);
            Assert.Equal(0, responseData.Data.First().MonthlyRentCost);
            Assert.Null(responseData.Data.First().FirstImage);
        }


        [Fact]
        public async Task GetAllPostWithPriceAndStatusAndTime_ReturnsNotFound_WhenNoPostsExist()
        {
            // Arrange
            var mockResponse = new Response<List<ListPostsResponseDto>>
            {
                Data = new List<ListPostsResponseDto>(), // Ensuring Data is an empty list, not null
                Succeeded = false,
                Message = "No posts found"
            };

            _postServiceMock.Setup(service => service.GetAllPostWithPriceAndStatusAndTime())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAllPostWithPriceAndStatusAndTime();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var responseData = Assert.IsType<Response<List<ListPostsResponseDto>>>(notFoundResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("No posts found", responseData.Message);
            Assert.Empty(responseData.Data); // Ensures the Data list is empty but not null
        }

        [Fact]
        public async Task GetAllPostWithPriceAndStatusAndTime_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _postServiceMock.Setup(service => service.GetAllPostWithPriceAndStatusAndTime())
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllPostWithPriceAndStatusAndTime();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var responseData = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.False(responseData.Succeeded);
            Assert.Equal("Internal server error: Unexpected error", responseData.Message);
        }

        [Fact]
        public async Task AddPost_ReturnsOkResult_WhenPostIsSuccessfullyAdded()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postDto = new AddPostRequestDto
            {
                HostelId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Title = "Sample Post",
                Description = "Description",
                DateAvailable = DateTime.Now
            };
            var mockResponse = new Response<AddPostRequestDto>(postDto)
            {
                Succeeded = true,
                Message = "Post added successfully"
            };

            _postServiceMock.Setup(service => service.AddPostAsync(postDto, It.IsAny<List<string>>(), userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddPost(userId, postDto, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Assert it's an OkObjectResult
            var responseData = Assert.IsType<Response<AddPostRequestDto>>(okResult.Value); // Assert correct response type
            Assert.True(responseData.Succeeded);
            Assert.Equal("Post added successfully", responseData.Message);
        }

        [Fact]
        public async Task AddPost_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postDto = new AddPostRequestDto(); // Missing required fields
            _controller.ModelState.AddModelError("Title", "The Title field is required.");

            // Act
            var result = await _controller.AddPost(userId, postDto, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value); // Validate SerializableError
            Assert.Contains("Title", modelState.Keys); // Check for the missing Title error
        }

        [Fact]
        public async Task AddPost_ReturnsBadRequest_WhenImageUploadFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postDto = new AddPostRequestDto
            {
                HostelId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Title = "Sample Post",
                Description = "Description",
                DateAvailable = DateTime.Now
            };
            var images = new List<IFormFile>
    {
        new Mock<IFormFile>().Object // Simulate invalid file
    };

            _s3ServiceMock.Setup(s3 => s3.UploadFileAsync(It.IsAny<IFormFile>()))
                .ThrowsAsync(new ArgumentException("File không hợp lệ."));

            // Act
            var result = await _controller.AddPost(userId, postDto, images);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Image upload failed: File không hợp lệ.", response.Message);
        }

        [Fact]
        public async Task AddPost_ReturnsBadRequest_WhenPostCountExceeded()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postDto = new AddPostRequestDto
            {
                HostelId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Title = "Sample Post",
                Description = "Description",
                DateAvailable = DateTime.Now
            };
            var mockResponse = new Response<AddPostRequestDto>
            {
                Succeeded = false,
                Message = "You have reached the maximum number of posts allowed for your membership."
            };

            _postServiceMock.Setup(service => service.AddPostAsync(postDto, It.IsAny<List<string>>(), userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddPost(userId, postDto, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("You have reached the maximum number of posts allowed for your membership.", response.Message);
        }

        [Fact]
        public async Task AddPost_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postDto = new AddPostRequestDto
            {
                HostelId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Title = "Sample Post",
                Description = "Description",
                DateAvailable = DateTime.Now
            };
            var images = new List<IFormFile>();

            _postServiceMock.Setup(service => service.AddPostAsync(It.IsAny<AddPostRequestDto>(), It.IsAny<List<string>>(), userId))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.AddPost(userId, postDto, images);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            var response = Assert.IsType<Response<string>>(internalServerErrorResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Internal server error: Unexpected error", response.Message);
        }

        [Theory]
        [InlineData("", "Valid Description", true, false, "The Title field is required.")] // Missing Title
        [InlineData("Valid Title", "", true, false, "The Description field is required.")] // Missing Description
        [InlineData("Valid Title", "Valid Description", false, false, "Invalid model state.")] // Invalid ModelState
        [InlineData("Valid Title", "Valid Description", true, true, null)] // Valid Data
        public async Task AddPost_WithDifferentData(string title, string description, bool isModelStateValid, bool expectedSuccess, string expectedError)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postDto = new AddPostRequestDto
            {
                HostelId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Title = title,
                Description = description,
                DateAvailable = DateTime.Now
            };

            if (!isModelStateValid)
            {
                _controller.ModelState.AddModelError("Key", expectedError ?? "Invalid model state.");
            }

            var mockResponse = new Response<AddPostRequestDto>
            {
                Succeeded = expectedSuccess,
                Message = expectedSuccess ? "Post added successfully" : expectedError
            };

            _postServiceMock
                .Setup(service => service.AddPostAsync(postDto, It.IsAny<List<string>>(), userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AddPost(userId, postDto, null);

            // Assert
            if (expectedSuccess)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var responseData = Assert.IsType<Response<AddPostRequestDto>>(okResult.Value);
                Assert.True(responseData.Succeeded);
                Assert.Equal("Post added successfully", responseData.Message);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                if (isModelStateValid)
                {
                    var response = Assert.IsType<Response<string>>(badRequestResult.Value);
                    Assert.False(response.Succeeded);
                    Assert.Equal(expectedError, response.Message);
                }
                else
                {
                    var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
                    Assert.Contains("Key", modelState.Keys);
                }
            }
        }

        //[Fact]
        //public async Task UpdatePost_ReturnsBadRequest_WhenImageUploadFails()
        //{
        //    // Arrange
        //    var postId = Guid.NewGuid();
        //    var request = new UpdatePostRequestDto
        //    {
        //        HostelId = Guid.NewGuid(),
        //        RoomId = Guid.NewGuid(),
        //        Title = "Updated Title",
        //        Description = "Updated Description",
        //        Status = true,
        //        DateAvailable = DateTime.Now.AddDays(10)
        //    };
        //    var images = new List<IFormFile> { new Mock<IFormFile>().Object };

        //    _s3ServiceMock.Setup(service => service.UploadFileAsync(It.IsAny<IFormFile>()))
        //        .ThrowsAsync(new ArgumentException("Invalid file"));

        //    // Act
        //    var result = await _controller.UpdatePost(postId, request, images);

        //    // Assert
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        //    var response = Assert.IsType<Response<string>>(badRequestResult.Value);
        //    Assert.False(response.Succeeded);
        //    Assert.Equal("Image upload failed: Invalid file", response.Message);
        //}

        //[Fact]
        //public async Task UpdatePost_ReturnsOkResult_WhenPostIsSuccessfullyUpdated()
        //{
        //    // Arrange
        //    var postId = Guid.NewGuid();
        //    var request = new UpdatePostRequestDto
        //    {
        //        HostelId = Guid.NewGuid(),
        //        RoomId = Guid.NewGuid(),
        //        Title = "Updated Title",
        //        Description = "Updated Description",
        //        Status = true,
        //        DateAvailable = DateTime.Now.AddDays(10)
        //    };
        //    var mockResponse = new Response<UpdatePostRequestDto>(request)
        //    {
        //        Succeeded = true,
        //        Message = "Post updated successfully"
        //    };

        //    // Mock the service to return a successful response
        //    _postServiceMock.Setup(service => service.UpdatePostAsync(postId, request, It.IsAny<List<string>>()))
        //        .ReturnsAsync(mockResponse);

        //    // Act
        //    var result = await _controller.UpdatePost(postId, request, null); // Pass null for images

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result); // Ensure it's OkObjectResult
        //    var response = Assert.IsType<Response<UpdatePostRequestDto>>(okResult.Value); // Ensure response is of the expected type
        //    Assert.True(response.Succeeded); // Assert success
        //    Assert.Equal("Post updated successfully", response.Message); // Check the message
        //}

        //[Fact]
        //public async Task UpdatePost_ReturnsBadRequest_WhenPostNotFound()
        //{
        //    // Arrange
        //    var postId = Guid.NewGuid();
        //    var request = new UpdatePostRequestDto
        //    {
        //        HostelId = Guid.NewGuid(),
        //        RoomId = Guid.NewGuid(),
        //        Title = "Updated Title",
        //        Description = "Updated Description",
        //        Status = true,
        //        DateAvailable = DateTime.Now.AddDays(10)
        //    };
        //    var mockResponse = new Response<UpdatePostRequestDto>
        //    {
        //        Succeeded = false,
        //        Message = "Post not found."
        //    };

        //    _postServiceMock
        //        .Setup(service => service.UpdatePostAsync(postId, request, It.IsAny<List<string>>()))
        //        .ReturnsAsync(mockResponse);

        //    // Act
        //    var result = await _controller.UpdatePost(postId, request, null);

        //    // Assert
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        //    var responseMessage = Assert.IsType<Response<UpdatePostRequestDto>>(badRequestResult.Value);
        //    Assert.False(responseMessage.Succeeded);
        //    Assert.Equal("Post not found.", responseMessage.Message);
        //}

        //[Fact]
        //public async Task UpdatePost_ReturnsBadRequest_WhenRoomIsInvalid()
        //{
        //    // Arrange
        //    var postId = Guid.NewGuid();
        //    var request = new UpdatePostRequestDto
        //    {
        //        HostelId = Guid.NewGuid(),
        //        RoomId = Guid.NewGuid(),
        //        Title = "Updated Title",
        //        Description = "Updated Description",
        //        Status = true,
        //        DateAvailable = DateTime.Now.AddDays(10)
        //    };
        //    var mockResponse = new Response<UpdatePostRequestDto>("The specified room does not belong to the hostel associated with this post.")
        //    {
        //        Succeeded = false
        //    };

        //    _postServiceMock.Setup(service => service.UpdatePostAsync(postId, request, It.IsAny<List<string>>()))
        //        .ReturnsAsync(mockResponse);

        //    // Act
        //    var result = await _controller.UpdatePost(postId, request, null);

        //    // Assert
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        //    var response = Assert.IsType<Response<UpdatePostRequestDto>>(badRequestResult.Value); // Adjusted to expect Response<UpdatePostRequestDto>
        //    Assert.False(response.Succeeded);
        //    Assert.Equal("The specified room does not belong to the hostel associated with this post.", response.Message);
        //}

        //[Fact]
        //public async Task UpdatePost_ReturnsBadRequest_WhenModelStateIsInvalid()
        //{
        //    // Arrange
        //    var postId = Guid.NewGuid();
        //    var request = new UpdatePostRequestDto(); // Missing required fields
        //    _controller.ModelState.AddModelError("Title", "The Title field is required.");

        //    // Act
        //    var result = await _controller.UpdatePost(postId, request, null);

        //    // Assert
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Check for BadRequestObjectResult
        //    var modelState = Assert.IsType<SerializableError>(badRequestResult.Value); // Ensure SerializableError is returned
        //    Assert.Contains("Title", modelState.Keys); // Validate that the error contains the expected field
        //}

        //[Fact]
        //public async Task UpdatePost_ReturnsInternalServerError_WhenExceptionIsThrown()
        //{
        //    // Arrange
        //    var postId = Guid.NewGuid();
        //    var request = new UpdatePostRequestDto
        //    {
        //        HostelId = Guid.NewGuid(),
        //        RoomId = Guid.NewGuid(),
        //        Title = "Updated Title",
        //        Description = "Updated Description",
        //        Status = true,
        //        DateAvailable = DateTime.Now.AddDays(10)
        //    };

        //    _postServiceMock.Setup(service => service.UpdatePostAsync(postId, request, It.IsAny<List<string>>()))
        //        .ThrowsAsync(new Exception("Unexpected error"));

        //    // Act
        //    var result = await _controller.UpdatePost(postId, request, null);

        //    // Assert
        //    var objectResult = Assert.IsType<ObjectResult>(result);
        //    Assert.Equal(500, objectResult.StatusCode);
        //    var response = Assert.IsType<Response<string>>(objectResult.Value);
        //    Assert.False(response.Succeeded);
        //    Assert.Equal("Internal server error: Unexpected error", response.Message);
        //}

        [Fact]
        public async Task DeletePost_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _postServiceMock
                .Setup(service => service.DeletePostAsync(postId, userId))
                .ThrowsAsync(new Exception("Unexpected error"));

            var claims = new List<Claim> { new Claim("UserId", userId.ToString()) };
            _controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims))
            };

            // Act
            var result = await _controller.DeletePost(postId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);

            var response = Assert.IsType<Response<string>>(objectResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Internal server error: Unexpected error", response.Message);
        }

        [Fact]
        public async Task DeletePost_ReturnsUnauthorized_WhenUserNotAuthenticated()
        {
            // Arrange
            var postId = Guid.NewGuid();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await _controller.DeletePost(postId);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Người dùng chưa được xác thực.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task DeletePost_ReturnsBadRequest_WhenPostDoesNotExist()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var mockResponse = new Response<bool>
            {
                Succeeded = false,
                Errors = new List<string> { "Bài đăng không tồn tại." }
            };

            _postServiceMock
                .Setup(service => service.DeletePostAsync(postId, userId))
                .ReturnsAsync(mockResponse);

            var claims = new List<Claim> { new Claim("UserId", userId.ToString()) };
            _controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims))
            };

            // Act
            var result = await _controller.DeletePost(postId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<List<string>>(badRequestResult.Value);
            Assert.Contains("Bài đăng không tồn tại.", errors);
        }

        [Fact]
        public async Task DeletePost_ReturnsBadRequest_WhenUserDoesNotHavePermission()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var mockResponse = new Response<bool>
            {
                Succeeded = false,
                Errors = new List<string> { "Bạn không có quyền xóa bài đăng này." }
            };

            _postServiceMock
                .Setup(service => service.DeletePostAsync(postId, userId))
                .ReturnsAsync(mockResponse);

            var claims = new List<Claim> { new Claim("UserId", userId.ToString()) };
            _controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims))
            };

            // Act
            var result = await _controller.DeletePost(postId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<List<string>>(badRequestResult.Value);
            Assert.Contains("Bạn không có quyền xóa bài đăng này.", errors);
        }

        [Fact]
        public async Task DeletePost_ReturnsOk_WhenPostIsSuccessfullyDeleted()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var mockResponse = new Response<bool>
            {
                Succeeded = true,
                Message = "Xóa bài đăng thành công."
            };

            _postServiceMock
                .Setup(service => service.DeletePostAsync(postId, userId))
                .ReturnsAsync(mockResponse);

            var claims = new List<Claim> { new Claim("UserId", userId.ToString()) };
            _controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims))
            };

            // Act
            var result = await _controller.DeletePost(postId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.Equal("Xóa bài đăng thành công.", response.Message);
        }

        [Fact]
        public async Task Get_ReturnsBadRequest_WhenInvalidQuery()
        {
            // Arrange
            var request = new GetAllPostsQuery
            {
                PageNumber = -1, // Invalid PageNumber
                PageSize = 0,    // Invalid PageSize
                SortBy = null
            };

            var mockResponse = new PagedResponse<List<ListPostsResponseDto>>
            {
                Succeeded = false,
                Errors = new List<string> { "Invalid query parameters." }
            };

            _postServiceMock.Setup(service => service.GetAllPostAysnc(request))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Get(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Contains("Invalid query parameters.", response.Errors);
        }

        [Fact]
        public async Task Get_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var request = new GetAllPostsQuery
            {
                SearchPhrase = "Keyword",
                PageNumber = 1,
                PageSize = 10,
                SortBy = "Title",
                SortDirection = SortDirection.Ascending
            };

            _postServiceMock.Setup(service => service.GetAllPostAysnc(request))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Get(request);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);

            var response = Assert.IsType<Response<string>>(serverErrorResult.Value);
            Assert.False(response.Succeeded);
            Assert.Contains("Internal server error", response.Message);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WhenPostsAreFound()
        {
            // Arrange
            var request = new GetAllPostsQuery
            {
                SearchPhrase = "Keyword",
                PageNumber = 1,
                PageSize = 10,
                SortBy = "Title",
                SortDirection = SortDirection.Ascending
            };

            var mockPosts = new List<ListPostsResponseDto>
    {
        new ListPostsResponseDto { Id = Guid.NewGuid(), Title = "Post 1" },
        new ListPostsResponseDto { Id = Guid.NewGuid(), Title = "Post 2" }
    };

            var mockResponse = PaginationHelper.CreatePagedResponse(mockPosts, request.PageNumber, request.PageSize, 2);

            _postServiceMock.Setup(service => service.GetAllPostAysnc(request))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Get(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PagedResponse<List<ListPostsResponseDto>>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.Equal(2, response.Data.Count);
        }

        [Fact]
        public async Task GetPostsByUserId_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _postServiceMock.Setup(service => service.GetPostsByUserIdAsync(userId))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetPostsByUserId(userId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);

            var response = Assert.IsType<Response<string>>(internalServerErrorResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Internal server error: Unexpected error", response.Message);
        }

        [Fact]
        public async Task GetPostsByUserId_ReturnsOkResult_WhenPostsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockPosts = new List<ListPostsResponseDto>
    {
        new ListPostsResponseDto { Id = Guid.NewGuid(), Title = "Post 1" },
        new ListPostsResponseDto { Id = Guid.NewGuid(), Title = "Post 2" }
    };
            var mockResponse = new Response<List<ListPostsResponseDto>>
            {
                Succeeded = true,
                Data = mockPosts
            };

            _postServiceMock.Setup(service => service.GetPostsByUserIdAsync(userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetPostsByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<Response<List<ListPostsResponseDto>>>(okResult.Value);
            Assert.True(responseData.Succeeded);
            Assert.Equal(2, responseData.Data.Count);
        }

        [Fact]
        public async Task GetPostsByUserId_ReturnsNotFound_WhenNoPostsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockResponse = new Response<List<ListPostsResponseDto>>
            {
                Succeeded = false,
                Errors = new List<string> { "Bạn chưa có bài đăng nào." }
            };

            _postServiceMock.Setup(service => service.GetPostsByUserIdAsync(userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetPostsByUserId(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.False(response.Succeeded);
            Assert.Contains("Bạn chưa có bài đăng nào.", response.Errors);
        }

        [Fact]
        public async Task GetPostsByUserId_ReturnsBadRequest_WhenUserIdIsInvalid()
        {
            // Arrange
            var userId = Guid.Empty;

            // Act
            var result = await _controller.GetPostsByUserId(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Invalid user ID.", response.Message);
        }

        [Fact]
        public async Task GetPostById_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var postId = Guid.NewGuid();

            _postServiceMock.Setup(service => service.GetPostByIdAsync(postId))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetPostById(postId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);

            var response = Assert.IsType<Response<string>>(internalServerErrorResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Internal server error: Unexpected error", response.Message);
        }

        [Fact]
        public async Task GetPostById_ReturnsBadRequest_WhenPostIdIsInvalid()
        {
            // Arrange
            var invalidPostId = Guid.Empty;

            // Act
            var result = await _controller.GetPostById(invalidPostId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Invalid post ID.", response.Message);
        }

        [Fact]
        public async Task GetPostById_ReturnsNotFound_WhenPostDoesNotExist()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var mockResponse = new Response<PostResponseDto>
            {
                Succeeded = false,
                Errors = new List<string> { "Bài đăng không tồn tại." }
            };

            _postServiceMock.Setup(service => service.GetPostByIdAsync(postId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetPostById(postId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.False(response.Succeeded);
            Assert.Contains("Bài đăng không tồn tại.", response.Errors);
        }

        [Fact]
        public async Task GetPostById_ReturnsOk_WhenPostExists()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var postResponse = new PostResponseDto
            {
                Id = postId,
                Title = "Sample Post"
            };
            var mockResponse = new Response<PostResponseDto>
            {
                Data = postResponse,
                Succeeded = true
            };

            _postServiceMock.Setup(service => service.GetPostByIdAsync(postId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetPostById(postId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<PostResponseDto>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.Equal(postId, response.Data.Id);
        }

        [Fact]
        public async Task FilterPosts_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var filter = new FilterPostsRequestDto
            {
                Province = "Province1"
            };

            _postServiceMock.Setup(service => service.FilterPostsAsync(filter))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.FilterPosts(filter);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);

            var response = Assert.IsType<Response<string>>(internalServerErrorResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Internal server error: Unexpected error", response.Message);
        }

        [Fact]
        public async Task FilterPosts_ReturnsBadRequest_WhenFilterIsInvalid()
        {
            // Arrange
            var filter = new FilterPostsRequestDto(); // Missing required fields
            _controller.ModelState.AddModelError("Province", "Province is required.");

            // Act
            var result = await _controller.FilterPosts(filter);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Invalid filter criteria.", response.Message);
        }

        [Fact]
        public async Task FilterPosts_ReturnsNotFound_WhenNoPostsMatchFilter()
        {
            // Arrange
            var filter = new FilterPostsRequestDto
            {
                Province = "NonExistentProvince",
                District = "NonExistentDistrict"
            };

            var mockResponse = new Response<List<ListPostsResponseDto>>
            {
                Data = new List<ListPostsResponseDto>(),
                Succeeded = true
            };

            _postServiceMock.Setup(service => service.FilterPostsAsync(filter))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.FilterPosts(filter);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("No posts found matching the filter criteria.", response.Message);
        }

        [Fact]
        public async Task FilterPosts_ReturnsOkResult_WhenPostsMatchFilter()
        {
            // Arrange
            var filter = new FilterPostsRequestDto
            {
                Province = "Province1",
                District = "District1",
                MinPrice = 1000,
                MaxPrice = 2000
            };

            var mockResponse = new Response<List<ListPostsResponseDto>>
            {
                Data = new List<ListPostsResponseDto>
        {
            new ListPostsResponseDto { Id = Guid.NewGuid(), Title = "Post 1", MonthlyRentCost = 1500 },
            new ListPostsResponseDto { Id = Guid.NewGuid(), Title = "Post 2", MonthlyRentCost = 1800 }
        },
                Succeeded = true
            };

            _postServiceMock.Setup(service => service.FilterPostsAsync(filter))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.FilterPosts(filter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<List<ListPostsResponseDto>>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.Equal(2, response.Data.Count);
        }

        [Fact]
        public async Task PushPost_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _postServiceMock.Setup(service => service.PushPostOnTopAsync(postId, It.IsAny<DateTime>(), userId))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.PushPost(postId, userId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);

            var response = Assert.IsType<Response<string>>(internalServerErrorResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Internal server error: Unexpected error", response.Message);
        }

        [Fact]
        public async Task PushPost_ReturnsBadRequest_WhenUserIdIsInvalid()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var invalidUserId = Guid.Empty;

            // Act
            var result = await _controller.PushPost(postId, invalidUserId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Invalid user ID.", response.Message);
        }

        [Fact]
        public async Task PushPost_ReturnsBadRequest_WhenPostNotFound()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mockResponse = new Response<PostResponseDto>
            {
                Succeeded = false,
                Message = "Post not found."
            };

            _postServiceMock.Setup(service => service.PushPostOnTopAsync(postId, It.IsAny<DateTime>(), userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.PushPost(postId, userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Post not found.", response.Message);
        }

        [Fact]
        public async Task PushPost_ReturnsBadRequest_WhenMembershipServiceNotInitialized()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mockResponse = new Response<PostResponseDto>
            {
                Succeeded = false,
                Message = "Membership service not initialized."
            };

            _postServiceMock.Setup(service => service.PushPostOnTopAsync(postId, It.IsAny<DateTime>(), userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.PushPost(postId, userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Membership service not initialized.", response.Message);
        }

        [Fact]
        public async Task PushPost_ReturnsOkResult_WhenPostIsSuccessfullyPushed()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mockResponse = new Response<PostResponseDto>
            {
                Succeeded = true,
                Message = "Post pushed to the top successfully.",
                Data = new PostResponseDto { Id = postId, Title = "Sample Post" }
            };

            _postServiceMock.Setup(service => service.PushPostOnTopAsync(postId, It.IsAny<DateTime>(), userId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.PushPost(postId, userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<PostResponseDto>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.Equal("Post pushed to the top successfully.", response.Message);
            Assert.Equal(postId, response.Data.Id);
        }

        [Fact]
        public async Task GetPostsOrderedByPriority_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _postServiceMock.Setup(service => service.GetPostsOrderedByPriorityAsync())
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetPostsOrderedByPriority();

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);

            var response = Assert.IsType<Response<string>>(internalServerErrorResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Internal server error: Unexpected error", response.Message);
        }

        [Fact]
        public async Task GetPostsOrderedByPriority_ReturnsNotFound_WhenNoPostsExist()
        {
            // Arrange
            var mockResponse = new Response<List<ListPostsResponseDto>>
            {
                Succeeded = true,
                Data = new List<ListPostsResponseDto>()
            };

            _postServiceMock.Setup(service => service.GetPostsOrderedByPriorityAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetPostsOrderedByPriority();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("No posts available.", response.Message);
        }

        [Fact]
        public async Task GetPostsOrderedByPriority_ReturnsOkResult_WhenPostsExist()
        {
            // Arrange
            var mockPosts = new List<ListPostsResponseDto>
    {
        new ListPostsResponseDto { Id = Guid.NewGuid(), Title = "Post 1", MonthlyRentCost = 1000 },
        new ListPostsResponseDto { Id = Guid.NewGuid(), Title = "Post 2", MonthlyRentCost = 2000 }
    };
            var mockResponse = new Response<List<ListPostsResponseDto>>
            {
                Succeeded = true,
                Data = mockPosts
            };

            _postServiceMock.Setup(service => service.GetPostsOrderedByPriorityAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetPostsOrderedByPriority();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<List<ListPostsResponseDto>>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.NotEmpty(response.Data);
            Assert.Equal(2, response.Data.Count);
        }

    }
}
