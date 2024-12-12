
using HostelFinder.Application.DTOs.AddressStory;
using HostelFinder.Application.DTOs.Story.Requests;
using HostelFinder.Application.DTOs.Story.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Enums;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace XUnitTestHostelFinder.Controllers
{
    public class StoryControllerTest
    {
        private readonly StoryController _controller;
        private readonly Mock<IStoryService> _storyServiceMock;

        public StoryControllerTest()
        {
            _storyServiceMock = new Mock<IStoryService>();
            _controller = new StoryController(_storyServiceMock.Object);
        }

        [Fact]
        public async Task AddStory_ShouldReturnOk_WhenStoryIsAddedSuccessfully()
        {
            // Arrange
            var request = new AddStoryRequestDto
            {
                UserId = Guid.NewGuid(),
                Title = "Nice Story",
                MonthlyRentCost = 500,
                Description = "A lovely house.",
                Size = 50,
                RoomType = RoomType.Chung_cư_mini,
                DateAvailable = DateTime.Now.AddDays(10),
                AddressStory = new AddressStoryDto
                {
                    Commune = "123 Main St",
                    Province = "Hanoi",
                    District = "District 1",
                    DetailAddress = "100000"
                },
                Images = new List<IFormFile>() // Assuming some mock images are provided
            };

            _storyServiceMock.Setup(service => service.AddStoryAsync(It.IsAny<AddStoryRequestDto>()))
                             .ReturnsAsync(new Response<string>
                             {
                                 Succeeded = true,
                                 Message = "Thêm bài viết thành công",
                                 Data = Guid.NewGuid().ToString()
                             });

            // Act
            var result = await _controller.AddStory(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<string>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.Equal("Thêm bài viết thành công", response.Message);
        }

        [Fact]
        public async Task AddStory_ShouldReturnBadRequest_WhenUserHasPosted5StoriesToday()
        {
            // Arrange
            var request = new AddStoryRequestDto
            {
                UserId = Guid.NewGuid(),
                Title = "Nice Story",
                MonthlyRentCost = 500,
                Description = "A lovely house.",
                Size = 50,
                RoomType = RoomType.Phòng_trọ,
                DateAvailable = DateTime.Now.AddDays(10),
                AddressStory = new AddressStoryDto
                {
                    Commune = "123 Main St",
                    Province = "Hanoi",
                    District = "District 1",
                    DetailAddress = "100000"
                },
                Images = new List<IFormFile>()
            };

            _storyServiceMock.Setup(service => service.AddStoryAsync(It.IsAny<AddStoryRequestDto>()))
                             .ReturnsAsync(new Response<string>
                             {
                                 Succeeded = false,
                                 Message = "Bạn đã đăng đủ 5 bài hôm nay. Vui lòng thử lại vào ngày mai."
                             });

            // Act
            var result = await _controller.AddStory(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Bạn đã đăng đủ 5 bài hôm nay. Vui lòng thử lại vào ngày mai.", response.Message);
        }

      

    }
}

