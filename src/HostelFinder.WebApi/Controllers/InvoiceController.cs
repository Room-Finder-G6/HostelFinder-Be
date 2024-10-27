using HostelFinder.Application.DTOs.InVoice.Requests;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var response = await _invoiceService.GetAllAsync();
            if (!response.Succeeded)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(Guid id)
        {
            var response = await _invoiceService.GetByIdAsync(id);
            if (!response.Succeeded)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] AddInVoiceRequestDto invoiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _invoiceService.CreateAsync(invoiceDto);
            if (!response.Succeeded)
                return BadRequest(response.Message);

            return CreatedAtAction(nameof(GetInvoice), new { id = response.Data.Id }, response.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] UpdateInvoiceRequestDto invoiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _invoiceService.UpdateAsync(id, invoiceDto);
            if (!response.Succeeded)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            var response = await _invoiceService.DeleteAsync(id);
            if (!response.Succeeded)
                return NotFound(response.Message);

            return NoContent();
        }
    }
}
