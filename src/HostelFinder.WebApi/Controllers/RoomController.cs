using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;
    
    public RoomController(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoomsByHostelId(Guid hostelId)
    {
        var rooms = await _roomRepository.GetRoomsByHostelId(hostelId);
        return Ok(rooms);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoomsByHostelId(Guid hostelId, string? search)
    {
        var rooms = await _roomRepository.GetRoomsByHostelId(hostelId, search);
        return Ok(rooms);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoomByRoomId(Guid roomId)
    {
        var room = await _roomRepository.GetRoomByRoomId(roomId);
        return Ok(room);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoomsByRoomType(string roomType)
    {
        var rooms = await _roomRepository.GetRoomsByRoomType(roomType);
        return Ok(rooms);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoomsByPrice(decimal minPrice, decimal maxPrice)
    {
        var rooms = await _roomRepository.GetRoomsByPrice(minPrice, maxPrice);
        return Ok(rooms);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoomsByTitle(string title)
    {
        var rooms = await _roomRepository.GetRoomsByTitle(title);
        return Ok(rooms);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddRoomAsync(Room room)
    {
        var addedRoom = await _roomRepository.AddRoomAsync(room);
        return Ok(addedRoom);
    }
}