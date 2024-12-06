using HostelFinder.Application.DTOs.MaintenanceRecord.Request;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers;
[ApiController]
[Route("api/maintenance-record")]
[Authorize("Landlord")]
public class MaintenanceRecordController : ControllerBase
{
    private readonly IMaintenanceRecordService _maintenanceRecordService;
    
    public MaintenanceRecordController(IMaintenanceRecordService maintenanceRecordService)
    {
        _maintenanceRecordService = maintenanceRecordService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddMaintenanceRecordAsync([FromBody] CreateMaintenanceRecordRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request is required.");
        }
        
        try
        {
            var response = await _maintenanceRecordService.AddMaintenanceRecordAsync(request);
            if (!response.Succeeded)
            {   
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllMaintenanceRecordAsync([FromQuery] GetAllMaintenanceRecordQuery request)
    {
        try
        {
            var response = await _maintenanceRecordService.GetAllMaintenanceRecordAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }
}