using System.Security.Claims;
using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/hostels")]
    [ApiController]
    public class HostelController : ControllerBase
    {
        private readonly IHostelService _hostelService;
        private readonly IS3Service _s3Service;
        
        public HostelController(IHostelService hostelService, IS3Service s3Service)
        {
            _hostelService = hostelService;
            _s3Service = s3Service;
        }

        [HttpGet("{hostelId}")]
        public async Task<IActionResult> GetHostelById(Guid hostelId)
        {
            var result = await _hostelService.GetHostelByIdAsync(hostelId);
            if (!result.Succeeded || result.Data == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("GetHostelsByLandlordId/{landlordId}")]
        public async Task<IActionResult> GetHostelsByLandlordId(Guid landlordId)
        {
            var hostels = await _hostelService.GetHostelsByUserIdAsync(landlordId);
            if (hostels.Succeeded)
            {
                return Ok(hostels);
            }

            return NotFound(hostels.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> AddHostel([FromForm] AddHostelRequestDto hostelDto, [FromForm] List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageUrls = new List<string>();

            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    var uploadToAWS3 = await _s3Service.UploadFileAsync(image);
                    var imageUrl = uploadToAWS3;
                    imageUrls.Add(imageUrl);
                }
            }
            
            try
            {
                var result = await _hostelService.AddHostelAsync(hostelDto, imageUrls);
                if (result.Succeeded)
                {
                    return CreatedAtAction(nameof(GetHostelById), new { hostelId = result.Data.Id }, result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{hostelId}")]
        public async Task<IActionResult> UpdateHostel(Guid hostelId, [FromForm] UpdateHostelRequestDto request, [FromForm] List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageUrls = new List<string>();

            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    var uploadToAWS3 = await _s3Service.UploadFileAsync(image);
                    var imageUrl = uploadToAWS3;
                    imageUrls.Add(imageUrl);
                }
            }

            var result = await _hostelService.UpdateHostelAsync(hostelId, request, imageUrls);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [HttpDelete("DeleteHostel/{id}")]
        public async Task<IActionResult> DeleteHostel(Guid id)
        {
            var result = await _hostelService.DeleteHostelAsync(id);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAll([FromBody] GetAllHostelQuery request)
        {
            var response = await _hostelService.GetAllHostelAsync(request);
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}