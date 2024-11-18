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
                var contract = await _rentalContractService.CreateRentalContractAsync(request);
                return Ok(contract);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
