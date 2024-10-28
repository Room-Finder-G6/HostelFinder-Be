using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            var response = await _roomService.GetAllAsync();
            if (!response.Succeeded)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(Guid id)
        {
            var response = await _roomService.GetByIdAsync(id);
            if (!response.Succeeded)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] AddRoomRequestDto roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var response = await _roomService.CreateAsync(roomDto);
                if (!response.Succeeded)
                    return BadRequest(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomRequestDto roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var response = await _roomService.UpdateAsync(id, roomDto);
                if (!response.Succeeded)
                    return NotFound(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid room ID");
            var response = await _roomService.DeleteAsync(id);
            if (!response.Succeeded)
                return NotFound(response.Message);

            return Ok(response);
        }
    }
}
