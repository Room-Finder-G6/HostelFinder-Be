using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.DTOs.Amenity.Response;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers;

[ApiController]
[Route("api/amenities")]
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
            return BadRequest(new Response<AmenityResponse>("Invalid model state"));
        }
        try
        {
            var response = await _amenityService.AddAmenityAsync(addAmenityDto);
            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

    }

    [HttpGet]
    public async Task<IActionResult> GetAllAmenities()
    {
        var response = await _amenityService.GetAllAmenitiesAsync();
        if (!response.Succeeded || response.Data == null || !response.Data.Any())
        {
            return NotFound(new Response<List<AmenityResponse>>("No amenities found"));
        }

        return Ok(response);
    }

    [HttpDelete("{amenityId}")]
    public async Task<IActionResult> DeleteAmenity(Guid amenityId)
    {
        var response = await _amenityService.DeleteAmenityAsync(amenityId);

        if (!response.Succeeded)
        {
            return NotFound(response);
        }
        return Ok(response);
    }


}