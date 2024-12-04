using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet("GetAllTenantsByHostel/{hostelId}")]
        public async Task<IActionResult> GetAllTenantsByHostelAsync(
            [FromRoute] Guid hostelId,
            [FromQuery] string? roomName,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _tenantService.GetAllTenantsByHostelAsync(hostelId, roomName, pageNumber, pageSize);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("moveout")]
        public async Task<IActionResult> MoveOut(Guid tenantId, Guid roomId)
        {
            if (tenantId == Guid.Empty || roomId == Guid.Empty)
            {
                return BadRequest(new Response<string>
                {
                    Succeeded = false,
                    Message = "Thông tin tenantId hoặc roomId không hợp lệ."
                });
            }

            var result = await _tenantService.MoveOutAsync(tenantId, roomId);

            if (!result.Succeeded)
            {
                return BadRequest(new Response<string>
                {
                    Succeeded = false,
                    Message = result.Message
                });
            }

            return Ok(result);
        }
    }
}
