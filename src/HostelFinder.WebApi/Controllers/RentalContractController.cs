using HostelFinder.Application.DTOs.RentalContract.Request;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/rental-contracts")]
    [ApiController]
    public class RentalContractController : ControllerBase
    {
        private readonly IRentalContractService _rentalContractService;
        public RentalContractController(IRentalContractService rentalContractService)
        {
            _rentalContractService = rentalContractService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRentalContract([FromForm] AddRentalContractDto request) { 
            try
            {
                var response = await _rentalContractService.CreateRentalContractAsync(request);
                if (!response.Succeeded)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
