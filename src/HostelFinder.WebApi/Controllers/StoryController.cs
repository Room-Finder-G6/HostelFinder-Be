using HostelFinder.Application.DTOs.Story.Requests;
using HostelFinder.Application.DTOs.Story.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {

        private readonly IStoryService _storyService;

        public StoryController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddStory([FromForm] AddStoryRequestDto request)
        {
            var response = await _storyService.AddStoryAsync(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoryById(Guid id)
        {
            var storyResponse = await _storyService.GetStoryByIdAsync(id);

            if (storyResponse == null)
            {
                return NotFound(new { message = "Bài đăng không tìm thấy." });
            }

            return Ok(storyResponse);
        }

        [HttpGet("GetAllStoryIndex")]
        public async Task<IActionResult> GetAllStoriesAsync([FromQuery] StoryFilterDto filter, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var response = await _storyService.GetAllStoriesAsync(pageIndex, pageSize, filter);

                if (response.Succeeded)
                {   
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<PagedResponse<ListStoryResponseDto>>
                {
                    Succeeded = false,
                    Message = $"Lỗi server: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpGet("GetAllStoryForAdmin")]
        public async Task<IActionResult> GetAllStoryForAdmin([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var pagedResponse = await _storyService.GetAllStoryForAdminAsync(pageIndex, pageSize);

            if (pagedResponse.Succeeded)
            {
                return Ok(pagedResponse);
            }

            return BadRequest(pagedResponse);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetStoryByUserId(Guid userId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _storyService.GetStoryByUserIdAsync(userId, pageIndex, pageSize);

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("{storyId}")]
        public async Task<IActionResult> DeleteStory(Guid storyId)
        {
            var response = await _storyService.DeleteStoryAsync(storyId);

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut("{storyId}")]
        public async Task<IActionResult> UpdateStory(Guid storyId, [FromForm] UpdateStoryRequestDto request, [FromForm] List<IFormFile>? images, [FromForm] List<string>? imageUrls)
        {
            if (request == null)
            {
                return BadRequest(new Response<StoryResponseDto>("Invalid input data."));
            }

            var response = await _storyService.UpdateStoryAsync(storyId, request, images, imageUrls);

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPatch("deny/{storyId}")]
        public async Task<IActionResult> DenyStory(Guid storyId)
        {
            var response = await _storyService.DenyStoryAsync(storyId);

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPatch("accept/{storyId}")]
        public async Task<IActionResult> AcceptStory(Guid storyId)
        {
            var response = await _storyService.AcceptStoryAsync(storyId);

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}
