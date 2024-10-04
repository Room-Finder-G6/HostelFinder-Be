using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostelController : ControllerBase
    {
        private readonly IHostelService _hostelService;

        public HostelController(IHostelService hostelService)
        {
            _hostelService = hostelService;
        }

        // GET: api/Hostel/GetHostelsByLandlordId/{landlordId}
        [HttpGet("GetHostelsByLandlordId/{landlordId}")]
        public async Task<IActionResult> GetHostelsByLandlordId(Guid landlordId)
        {
            var hostels = await _hostelService.GetHostelsByLandlordIdAsync(landlordId);
            return Ok(hostels);
        }

        // POST: api/Hostel/AddHostel
        [HttpPost("AddHostel")]
        public async Task<IActionResult> AddHostel([FromBody] AddHostelRequestDto hostelDto)
        {
            var result = await _hostelService.AddHostelAsync(hostelDto);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
        }

        // PUT: api/Hostel/hostelId
        [HttpPut("UpdateHostel/{hostelId}")]
        public async Task<IActionResult> UpdateHostel(Guid hostelId, [FromBody] UpdateHostelRequestDto hostelDto)
        {
            var result = await _hostelService.UpdateHostelAsync(hostelDto);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return NotFound(result.Errors);
        }

        // DELETE: api/Hostel/DeleteHostel/{id}
        [HttpDelete("DeleteHostel/{id}")]
        public async Task<IActionResult> DeleteHostel(Guid id)
        {
            var result = await _hostelService.DeleteHostelAsync(id);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return NotFound(result.Errors);
        }
    }
}
