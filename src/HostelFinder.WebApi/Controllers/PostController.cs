using HostelFinder.Application.DTOs.Post.Requests;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers;

[ApiController]
[Route("api/posts/")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IS3Service _s3Service;

    public PostController(IPostService postService, IS3Service s3Service)
    {
        _postService = postService;
        _s3Service = s3Service;
    }

    /*[HttpGet]
    [Route("{postId}")]
    public async Task<IActionResult> GetAllPostFeaturesByPostId(Guid roomId)
    {
        var result = await _postService.GetAllRoomFeaturesByIdAsync(roomId);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound();
    }*/

    [HttpPost]
    public async Task<IActionResult> AddPost(Guid userId, [FromForm] AddPostRequestDto postDto,
        [FromForm] List<IFormFile> images)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Upload image to AWS and collect Image response
        var imageUrls = new List<string>();

        if (images != null && images.Count > 0)
        {
            foreach (var image in images)
            {
                var imageUrl = await _s3Service.UploadFileAsync(image);
                imageUrls.Add(imageUrl);
            }
        }

        // Pass userId directly to the AddPostAsync method
        var result = await _postService.AddPostAsync(postDto, imageUrls, userId);

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result.Message);
    }

    [HttpPut("{postId}")]
    public async Task<IActionResult> UpdatePost(Guid postId, [FromForm] UpdatePostRequestDto request, [FromForm] List<IFormFile> images)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var imageUrls = new List<string>();

        if (images != null && images.Count > 0)
        {
            foreach (var image in images)
            {
                var imageUrl = await _s3Service.UploadFileAsync(image);
                imageUrls.Add(imageUrl);
            }
        }

        var result = await _postService.UpdatePostAsync(postId, request, imageUrls);

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result.Message);
    }


    [HttpDelete]
    [Route("{postId}")]
    public async Task<IActionResult> DeletePost(Guid postId)
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim == null)
        {
            return Unauthorized("Người dùng chưa được xác thực.");
        }

        var currentUserId = Guid.Parse(userIdClaim.Value);
        var response = await _postService.DeletePostAsync(postId, currentUserId);
        if (!response.Succeeded)
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Message);
    }

    [HttpPost("get-all")]
    public async Task<IActionResult> Get(GetAllPostsQuery request)
    {
        var response = await _postService.GetAllPostAysnc(request);
        if (response.Succeeded)
        {
            return BadRequest(response.Errors);
        }

        return Ok(response);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetPostsByUserId(Guid userId)
    {
        var result = await _postService.GetPostsByUserIdAsync(userId);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound(result.Errors);
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPostById(Guid postId)
    {
        var result = await _postService.GetPostByIdAsync(postId);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound();
    }

    [HttpPost("filter")]
    public async Task<IActionResult> FilterPosts([FromBody] FilterPostsRequestDto filter)
    {
        var result = await _postService.FilterPostsAsync(filter);

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result.Errors);
    }

    [HttpPatch]
    [Route("{postId}/push")]
    public async Task<IActionResult> PushPost(Guid postId, Guid userId)
    {
        var result = await _postService.PushPostOnTopAsync(postId, DateTime.Now, userId);

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(new { Message = result.Message });
    }

    [HttpGet("ordered")]
    public async Task<IActionResult> GetPostsOrderedByPriority()
    {
        var result = await _postService.GetPostsOrderedByPriorityAsync();

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result.Errors);
    }

}