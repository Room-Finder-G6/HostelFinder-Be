using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
   private readonly IRevenueReportService _revenueReportService;
   
   public ReportController(IRevenueReportService revenueReportService)
   {
       _revenueReportService = revenueReportService;
   }
   
   [HttpGet]
   [Route("/yearly-revenue")]
    public async Task<IActionResult> GetYearlyRevenueReport(Guid hostelId, int year)
    {
         try
         {
              var response = await _revenueReportService.GetYearlyRevenueReportByHostel(hostelId, year);
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
    
    [HttpGet]
     [Route("/monthly-revenue")]
    
    public async Task<IActionResult> GetMonthlyRevenueReport(Guid hostelId, int month, int year)
    {
         try
         {
              var response = await _revenueReportService.GetMonthlyRevenueReportByHostel(hostelId, month, year);
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