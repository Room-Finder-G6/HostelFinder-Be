using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    [Route("GetAllRoomFeaturesByRoomId/{roomId}")]
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
    [Route("AddRoom")]
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
    [Route("UpdateRoom/{roomId}")]
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
    [Route("DeleteRoom/{roomId}")]
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
    public async Task<IActionResult> GetFilteredRooms(decimal? minPrice, decimal? maxPrice, string? location)
    {
        var result = await _roomService.GetFilteredRooms(minPrice, maxPrice, location);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound();
    }
}