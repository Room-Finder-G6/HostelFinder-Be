using HostelFinder.Application.DTOs.Story.Requests;
using HostelFinder.Application.Interfaces.IServices;
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
        public async Task<IActionResult> GetAllStory()
        {
            var response = await _storyService.GetAllStoryAsync();

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("GetAllStoryForAdmin")]
        public async Task<IActionResult> GetAllStoryForAdmin()
        {
            var response = await _storyService.GetAllStoryForAdminAsync();

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetStoryByUserId(Guid userId)
        {
            var response = await _storyService.GetStoryByUserIdAsync(userId);

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
