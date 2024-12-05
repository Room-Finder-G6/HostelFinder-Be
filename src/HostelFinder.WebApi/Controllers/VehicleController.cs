using HostelFinder.Application.DTOs.Vehicle.Request;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("tenant/{tenantId}")]
        [Authorize(Roles = "Landlord,Admin")]
        public async Task<IActionResult> GetVehicleByTenant(Guid tenantId)
        {
            var response = await _vehicleService.GetVehicleByTenantAsync(tenantId);
            if (response.Succeeded)
                return Ok(response);
            else
                return NotFound(response);
        }

        // Thêm xe mới
        [HttpPost]
        [Authorize(Roles = "Landlord,Admin")]
        public async Task<IActionResult> AddVehicle([FromForm] AddVehicleDto request)
        {
            var response = await _vehicleService.AddVehicleAsync(request);
            if (response.Succeeded)
                return Ok(response);
            else
                return BadRequest(response);
        }

        // Lấy thông tin xe theo ID
        [HttpGet("{vehicleId}")]
        public async Task<IActionResult> GetVehicleById(Guid vehicleId)
        {
            var response = await _vehicleService.GetVehicleByIdAsync(vehicleId);
            if (response.Succeeded)
                return Ok(response);
            else
                return NotFound(response);
        }

        // Lấy danh sách tất cả xe
        [HttpGet]
        [Authorize(Roles = "Landlord,Admin")]
        public async Task<IActionResult> GetAllVehicles()
        {
            var response = await _vehicleService.GetAllVehiclesAsync();
            return Ok(response);
        }

        // Cập nhật xe
        [HttpPut("{vehicleId}")]
        [Authorize(Roles = "Landlord,Admin")]
        public async Task<IActionResult> UpdateVehicle(Guid vehicleId, [FromForm] AddVehicleDto request)
        {
            var response = await _vehicleService.UpdateVehicleAsync(vehicleId, request);
            if (response.Succeeded)
                return Ok(response);
            else
                return BadRequest(response);
        }

        // Xóa xe
        [HttpDelete("{vehicleId}")]
        [Authorize(Roles = "Landlord,Admin")]
        public async Task<IActionResult> DeleteVehicle(Guid vehicleId)
        {
            var response = await _vehicleService.DeleteVehicleAsync(vehicleId);
            if (response.Succeeded)
                return Ok(response);
            else
                return NotFound(response);
        }
    }
}
