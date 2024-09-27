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
        var result = await _roomService.AddRoomAsync(roomDto);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest();
    }
}