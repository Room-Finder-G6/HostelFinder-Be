using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Filter;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private readonly IPostService _postService;

    public RoomController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    [Route("{roomId}")]
    public async Task<IActionResult> GetAllRoomFeaturesByRoomId(Guid roomId)
    {
        var result = await _postService.GetAllRoomFeaturesByIdAsync(roomId);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddRoom([FromBody] AddPostRequestDto postDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _postService.AddRoomAsync(postDto);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result.Errors);
    }

    [HttpPut]
    [Route("{roomId}")]
    public async Task<IActionResult> UpdateRoom([FromBody] UpdatePostRequestDto postDto, Guid roomId)
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
    }

    [HttpDelete]
    [Route("{roomId}")]
    public async Task<IActionResult> DeleteRoom(Guid roomId)
    {
        var result = await _postService.DeleteRoomAsync(roomId);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound();
    }

    [HttpGet("{postId}/landlord")]
    public async Task<IActionResult> GetLandlordByPostId(Guid postId)
    {
        var landlord = await _postService.GetLandlordByPostIdAsync(postId);

        if (landlord == null)
        {
            return NotFound("Post or associated landlord not found.");
        }

        return Ok(landlord);
    }

}