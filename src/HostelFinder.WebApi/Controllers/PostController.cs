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
    public async Task<IActionResult> AddPost([FromForm] AddPostRequestDto postDto, [FromForm] List<IFormFile> images)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //Upload image to AWS and collect Url response
        var imageUrls = new List<string>();

        if (images != null && images.Count > 0)
        {
            foreach (var image in images)
            {
                var uploadToAWS3 = await _s3Service.UploadFileAsync(image);
                var imageUrl = uploadToAWS3;
                imageUrls.Add(imageUrl);
            }
        }

        var result = await _postService.AddPostAsync(postDto, imageUrls);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result.Errors);
    }

    /*[HttpPut]
    [Route("{postId}")]
    public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequestDto postDto, Guid roomId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _postService.UpdateRoomAsync(postDto, roomId);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result.Errors);
    }*/

    [HttpDelete]
    [Route("{postId}")]
    public async Task<IActionResult> DeletePost(Guid postId)
    {
        var result = await _postService.DeletePostAsync(postId);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound(result.Errors);
    }

    /*[HttpGet("{postId}/landlord")]
    public async Task<IActionResult> GetLandlordByPostId(Guid postId)
    {
        var landlord = await _postService.GetLandlordByPostIdAsync(postId);

        if (landlord == null)
        {
            return NotFound("Post or associated landlord not found.");
        }

        return Ok(landlord);
    }*/

    /*[HttpGet("{postId}/hostel")]
    public async Task<IActionResult> GetHostelByPostId(Guid postId)
    {
        var hostel = await _postService.GetHostelByPostIdAsync(postId);

        if (hostel == null)
        {
            return NotFound("Hostel not found.");
        }

        return Ok(hostel);
    }*/

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

    [HttpPost("test-upload-file")]
    public async Task<IActionResult> TestUploadFile(IFormFile file)
    {
        try
        {
            var fileUrl = await _s3Service.UploadFileAsync(file);
            return Ok(new { Url = fileUrl });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetPostByUserId(Guid userId)
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
}