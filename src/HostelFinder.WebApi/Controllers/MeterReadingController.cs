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
                var meterReaded = await _meterReadingService.AddMeterReadingAsync(createMeterReadingDto.roomId, createMeterReadingDto.serviceId, createMeterReadingDto.reading,createMeterReadingDto.billingMonth, createMeterReadingDto.billingYear);
                return Ok(meterReaded);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
