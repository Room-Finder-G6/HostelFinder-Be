using HostelFinder.Application.DTOs.MeterReading.Request;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/meterReadings")]
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterReadingService _meterReadingService;
        public MeterReadingController(IMeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }

        [HttpPost]
        public async Task<IActionResult> AddMeterReading([FromBody] CreateMeterReadingDto createMeterReadingDto)
        {
            try
            {
                var meterReaded = await _meterReadingService.AddMeterReadingAsync(createMeterReadingDto.roomId, createMeterReadingDto.serviceId, createMeterReadingDto.previousReading, createMeterReadingDto.currentReading,createMeterReadingDto.billingMonth, createMeterReadingDto.billingYear);
                return Ok(meterReaded);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("list")]
        public async Task<IActionResult> AddMeterReadingList([FromBody] List<CreateMeterReadingDto> createMeterReadingDtos)
        {
            try
            {
                var response = await _meterReadingService.AddMeterReadingListAsync(createMeterReadingDtos);
                if (!response.Succeeded)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("{hostelId}/{roomId}")]
        public async Task<IActionResult> GetServiceCostReading(Guid hostelId, Guid roomId, int? billingMonth, int? billingYear)
        {
            try
            {
                var serviceCostReading = await _meterReadingService.GetServiceCostReadingAsync(hostelId, roomId, billingMonth, billingYear);
                return Ok(serviceCostReading);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
