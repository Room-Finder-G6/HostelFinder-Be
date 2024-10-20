using HostelFinder.Application.DTOs.Post.Requests;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpPost]
        public async Task<IActionResult> Get(GetAllPostsQuery request)
        {
            var response = await _postService.GetAllPostAysnc(request);
            if (response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
