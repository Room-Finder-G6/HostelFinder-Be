using HostelFinder.Application.DTOs.ServiceCost.Request;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceCostController : ControllerBase
    {
        private readonly IServiceCostService _serviceCostService;

        public ServiceCostController(IServiceCostService serviceCostService)
        {
            _serviceCostService = serviceCostService;
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceCosts()
        {
            var response = await _serviceCostService.GetAllAsync();
            if (!response.Succeeded)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceCost(Guid id)
        {
            var response = await _serviceCostService.GetByIdAsync(id);
            if (!response.Succeeded)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceCost([FromBody] AddServiceCostDto serviceCostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _serviceCostService.CreateAsync(serviceCostDto);
                if (!response.Succeeded)
                    return BadRequest(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceCost(Guid id, [FromBody] UpdateServiceCostDto serviceCostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _serviceCostService.UpdateAsync(id, serviceCostDto);
            if (!response.Succeeded)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceCost(Guid id)
        {
            var response = await _serviceCostService.DeleteAsync(id);
            if (!response.Succeeded)
                return NotFound(response.Message);

            return Ok(response);
        }

    }

}
