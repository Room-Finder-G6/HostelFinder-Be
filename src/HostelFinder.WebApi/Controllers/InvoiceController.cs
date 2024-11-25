﻿using HostelFinder.Application.DTOs.InVoice.Requests;
using HostelFinder.Application.DTOs.InVoice.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/invoices")]
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
            try
            {
                var response = await _invoiceService.GetAllAsync();
                if (!response.Succeeded)
                {
                    return BadRequest(new Response<List<InvoiceResponseDto>>
                    {
                        Succeeded = false,
                        Message = response.Message
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<InvoiceResponseDto>>
                {
                    Succeeded = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new Response<InvoiceResponseDto>
                    {
                        Succeeded = false,
                        Message = "Invalid ID",
                        Data = null
                    });
                }

                var response = await _invoiceService.GetByIdAsync(id);
                if (!response.Succeeded || response.Data == null)
                {
                    return NotFound(new Response<InvoiceResponseDto>
                    {
                        Succeeded = false,
                        Message = response.Message
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<InvoiceResponseDto>
                {
                    Succeeded = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }


        [HttpPost]
        [Route("monthly-invoice")]
        public async Task<IActionResult> CreateInvoice([FromBody] AddInVoiceRequestDto invoiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response<InvoiceResponseDto>
                {
                    Succeeded = false,
                    Message = "Invalid model state."
                });

            try
            {
                var response = await _invoiceService.GenerateMonthlyInvoicesAsync(invoiceDto.roomId, invoiceDto.billingMonth, invoiceDto.billingYear);
                if (!response.Succeeded)
                {
                    return new ObjectResult(response)
                    {
                        StatusCode = 400
                    };
                }

                return new ObjectResult(response)
                {
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ObjectResult(new Response<InvoiceResponseDto>
                {
                    Succeeded = false,
                    Message = $"{ex.Message}"
                })
                {
                    StatusCode = 500
                };
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(Guid id, [FromForm] UpdateInvoiceRequestDto invoiceDto)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new Response<string>
                    {
                        Succeeded = false,
                        Message = "Invalid invoice ID"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _invoiceService.UpdateAsync(id, invoiceDto);
                if (!response.Succeeded)
                {
                    return NotFound(new Response<string>
                    {
                        Succeeded = false,
                        Message = response.Message
                    });
                }

                return Ok(response);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            try
            {
                var response = await _invoiceService.DeleteAsync(id);
                if (!response.Succeeded)
                {
                    if (response.Message == "Invoice not found.")
                    {
                        return NotFound(new Response<bool>
                        {
                            Succeeded = false,
                            Message = response.Message
                        });
                    }

                    return BadRequest(new Response<bool>
                    {
                        Succeeded = false,
                        Message = response.Message
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>
                {
                    Succeeded = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }


    }
}
