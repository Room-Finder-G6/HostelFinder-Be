using HostelFinder.Application.DTOs.RentalContract.Request;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> CreateRentalContract([FromForm] AddRentalContractDto request)
        {
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

        [HttpPost]
        [Route("termination-of-contract")]
        public async Task<IActionResult> TerminationOfContract([FromBody] Guid rentalContractId)
        {
            try
            {
                var response = await _rentalContractService.TerminationOfContract(rentalContractId);
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
