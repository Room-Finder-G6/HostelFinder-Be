using System.Security.Claims;
using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/hostels")]
    [ApiController]
    public class HostelController : ControllerBase
    {
        private readonly IHostelService _hostelService;

        public HostelController(IHostelService hostelService)
        {
            _hostelService = hostelService;
        }

        [HttpGet("{hostelId}")]
        public async Task<IActionResult> GetHostelById(Guid hostelId)
        {
            var result = await _hostelService.GetHostelByIdAsync(hostelId);
            if (!result.Succeeded || result.Data == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("GetHostelsByLandlordId/{landlordId}")]
        public async Task<IActionResult> GetHostelsByLandlordId(Guid landlordId)
        {
            var hostels = await _hostelService.GetHostelsByUserIdAsync(landlordId);
            if (hostels.Succeeded)
            {
                return Ok(hostels);
            }

            return NotFound(hostels.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> AddHostel([FromBody] AddHostelRequestDto hostelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _hostelService.AddHostelAsync(hostelDto);
                if (result.Succeeded)
                {
                    return CreatedAtAction(nameof(GetHostelById), new { hostelId = result.Data.Id }, result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("UpdateHostel/{hostelId}")]
        public async Task<IActionResult> UpdateHostel(Guid hostelId, [FromBody] UpdateHostelRequestDto hostelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim not found in token.");
            }

            var userId = Guid.Parse(userIdClaim.Value);


            var result = await _hostelService.UpdateHostelAsync(hostelId, userId, hostelDto);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return NotFound(new { message = "Hostel not found or update failed.", errors = result.Errors });
        }

        [HttpDelete("DeleteHostel/{id}")]
        public async Task<IActionResult> DeleteHostel(Guid id)
        {
            var result = await _hostelService.DeleteHostelAsync(id);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAll([FromBody] GetAllHostelQuery request)
        {
            var response = await _hostelService.GetAllHostelAsync(request);
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}