﻿using HostelFinder.Application.DTOs.ServiceCost.Request;
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

        [HttpPost]
        public async Task<IActionResult> CreateServiceCost(AddServiceCostDto dto)
        {
            var result = await _serviceCostService.AddServiceCostAsync(dto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceCost(Guid id, UpdateServiceCostDto dto)
        {
            var result = await _serviceCostService.UpdateServiceCostAsync(id, dto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceCost(Guid id)
        {
            var result = await _serviceCostService.DeleteServiceCostAsync(id);
            if (!result.Succeeded)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetServiceCostByPostId(Guid postId)
        {
            var serviceCosts = await _serviceCostService.GetServiceCostsByPostIdAsync(postId);
            return Ok(serviceCosts);
        }
    }

}
