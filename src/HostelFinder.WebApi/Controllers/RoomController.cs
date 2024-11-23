using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.Room.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly ITenantService _tenantService;
        public RoomController(IRoomService roomService,
            ITenantService tenantService)
        {
            _roomService = roomService;
            _tenantService = tenantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            try
            {
                var response = await _roomService.GetAllAsync();

                if (!response.Succeeded)
                {
                    return BadRequest(new Response<List<RoomResponseDto>>
                    {
                        Succeeded = false,
                        Message = response.Message ?? "Failed to retrieve rooms."
                    });
                }

                if (response.Data == null || !response.Data.Any())
                {
                    return Ok(new Response<List<RoomResponseDto>>
                    {
                        Succeeded = true,
                        Data = new List<RoomResponseDto>(),
                        Message = "No rooms found"
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Succeeded = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(Guid id)
        {
            try
            {
                var response = await _roomService.GetByIdAsync(id);
                if (!response.Succeeded)
                    return NotFound(response.Message);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromForm] AddRoomRequestDto roomDto, [FromForm] List<IFormFile> roomImages)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //map to Domain Room 
                var response = await _roomService.CreateRoomAsync(roomDto, roomImages);
                if (!response.Succeeded)
                    return BadRequest(response);

                return Ok(response);
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
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _roomService.UpdateAsync(id, roomDto);
                if (!response.Succeeded)
                    return BadRequest(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new Response<string>
                    {
                        Succeeded = false,
                        Message = "Invalid room ID"
                    });
                }

                var response = await _roomService.DeleteAsync(id);

                if (!response.Succeeded)
                {
                    return NotFound(new Response<string>
                    {
                        Succeeded = false,
                        Message = response.Message
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Succeeded = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [HttpGet("hostels/{hostelId}")]
        public async Task<IActionResult> GetRoomsByHostelId(Guid hostelId, int? floor)
        {
            try
            {
                var response = await _roomService.GetRoomsByHostelIdAsync(hostelId, floor);
                if (!response.Succeeded)
                    return BadRequest(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Succeeded = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [HttpGet]
        [Route("info-retancy")]
        public async Task<IActionResult> GetInfoRentacyRoom(Guid roomId)
        {
            var response = await _tenantService.GetInformationTenacyAsync(roomId);
            return Ok(response);
        }

        [HttpGet]
        [Route("info-detail")]
        public async Task<IActionResult> GetInfoDetailRoom(Guid roomId)
        {
            var response = await _roomService.GetInformationDetailRoom(roomId);
            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
