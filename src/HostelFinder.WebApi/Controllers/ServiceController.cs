using HostelFinder.Application.DTOs.Service.Request;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceService.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
                return NotFound();

            return Ok(service);
        }

        [HttpPost]
        public async Task<IActionResult> AddService(ServiceCreateRequestDTO serviceCreateRequestDTO)
        {
            await _serviceService.AddServiceAsync(serviceCreateRequestDTO);
            return CreatedAtAction(nameof(GetServiceById), new { id = serviceCreateRequestDTO.HostelId }, serviceCreateRequestDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(Guid id, ServiceUpdateRequestDTO serviceUpdateRequestDTO)
        {
            await _serviceService.UpdateServiceAsync(id, serviceUpdateRequestDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            await _serviceService.DeleteServiceAsync(id);
            return NoContent();
        }
    }
}
