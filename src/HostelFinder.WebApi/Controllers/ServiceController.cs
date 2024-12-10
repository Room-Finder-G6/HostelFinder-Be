using HostelFinder.Application.DTOs.Service.Request;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [Authorize(Roles = "Landlord,Admin")]
        public async Task<IActionResult> GetAllServices()
        {
            try
            {
                var services = await _serviceService.GetAllServicesAsync();

                if (services.Data == null || !services.Data.Any())
                {
                    return NotFound(new Response<string>
                    {
                        Succeeded = false,
                        Message = "No services available."
                    });
                }

                return Ok(services);
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

        [HttpGet("GetServiceById/{id}")]
        [Authorize(Roles = "Landlord,Admin")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
                return NotFound();

            return Ok(service);
        }

        [HttpPost("AddService")]
        [Authorize(Roles = "Landlord,Admin")]
        public async Task<IActionResult> AddService(ServiceCreateRequestDTO serviceCreateRequestDTO)
        {
            var response = await _serviceService.AddServiceAsync(serviceCreateRequestDTO);
            if (response.Succeeded)
            {
                return Ok(response); 
            }
            return BadRequest(response.Message); 
        }

        [HttpPut("UpdateService/{id}")]
        [Authorize(Roles = "Landlord,Admin")]
        public async Task<IActionResult> UpdateService(Guid id, ServiceUpdateRequestDTO serviceUpdateRequestDTO)
        {
            var response = await _serviceService.UpdateServiceAsync(id, serviceUpdateRequestDTO);
            if (response.Succeeded)
            {
                return NoContent();
            }
            return NotFound(response.Message);
        }

        [HttpDelete("DeleteService/{id}")]
        [Authorize(Roles = "Landlord,Admin")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var response = await _serviceService.DeleteServiceAsync(id);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response.Message);
        }

        [HttpGet]
        [Authorize(Roles = "Landlord,Admin")]
        [Route("hostels/{hostelId}")]
        public async Task<IActionResult> GetServiceByHostel(Guid hostelId)
        {
            var response = await _serviceService.GetAllServiceByHostelAsync(hostelId);
            if(!response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
