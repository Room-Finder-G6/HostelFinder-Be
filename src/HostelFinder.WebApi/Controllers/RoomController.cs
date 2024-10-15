using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;
private readonly IS3Service _s3Service;
    
    public RoomController(IRoomService roomService, IS3Service s3Service)
    {
        _roomService = roomService;
        _s3Service = s3Service;
    }

    [HttpGet]
    [Route("{roomId}")]
    public async Task<IActionResult> GetAllRoomFeaturesByRoomId(Guid roomId)
    {
        var result = await _roomService.GetAllRoomFeaturesByIdAsync(roomId);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomRequestDto roomDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _roomService.AddRoomAsync(roomDto);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result.Errors);
    }

    [HttpPut]
    [Route("{roomId}")]
    public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomRequestDto roomDto, Guid roomId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _roomService.UpdateRoomAsync(roomDto, roomId);
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
        var result = await _roomService.DeleteRoomAsync(roomId);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound();
    }

    [HttpGet]
    [Route("GetFilteredRooms")]
    public async Task<IActionResult> GetFilteredRooms(decimal? minPrice, decimal? maxPrice, string? location, RoomType roomType)
    {
        var result = await _roomService.GetFilteredRooms(minPrice, maxPrice, location, roomType);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound();
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
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
}