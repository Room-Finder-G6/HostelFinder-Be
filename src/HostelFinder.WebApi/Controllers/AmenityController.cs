using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class AmenityController : ControllerBase
{
    private readonly IAmenityService _amenityService;
    
    public AmenityController(IAmenityService amenityService)
    {
        _amenityService = amenityService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAmenity([FromBody] AddAmenityDto addAmenityDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var amenity = await _amenityService.AddAmenityAsync(addAmenityDto);
            return Ok(amenity);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet]
    [Route("amenities")]
    public async Task<IActionResult> GetAllAmenities()
    {
        var amenities = await _amenityService.GetAllAmenitiesAsync();
        if(amenities == null)
        {
            return NotFound("No amenities found");
        }
        return Ok(amenities);
    }
    
    [HttpDelete("amenityId")]
    public async Task<IActionResult> DeleteAmenity(Guid amenityId)
    {
        var result = await _amenityService.DeleteAmenityAsync(amenityId);
        if (result.Succeeded)
        {
            return Ok(result);
        }
        return NotFound("Amenity not found");
    }
}