﻿using HostelFinder.Application.DTOs.Hostel.Requests;
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
            return Ok(result);
        }

        // GET: api/Hostel/GetHostelsByLandlordId/{landlordId}
        [HttpGet("GetHostelsByLandlordId/{landlordId}")]
        public async Task<IActionResult> GetHostelsByLandlordId(Guid landlordId)
        {
            var hostels = await _hostelService.GetHostelsByLandlordIdAsync(landlordId);
            return Ok(hostels);
        }

        // POST: api/Hostel/AddHostel
        [HttpPost()]
        public async Task<IActionResult> AddHostel([FromBody] AddHostelRequestDto hostelDto)
        {
            var result = await _hostelService.AddHostelAsync(hostelDto);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
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

        [HttpPost]
        [Route("get-all")]
        public async Task<IActionResult> GetAll(GetAllHostelQuery request)
        {
            var response = await _hostelService.GetAllHostelAsync(request);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}