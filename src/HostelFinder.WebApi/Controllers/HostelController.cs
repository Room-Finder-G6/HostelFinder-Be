using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Common.Constants;
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
            try
            {
                var result = await _hostelService.GetHostelByIdAsync(hostelId);
                if (!result.Succeeded || result.Data == null)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<HostelResponseDto>
                {
                    Succeeded = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("GetHostelsByLandlordId/{landlordId}")]
        public async Task<IActionResult> GetHostelsByLandlordId(Guid landlordId, string? searchPhrase, int? pageNumber, int? pageSize,string? sortBy, SortDirection? sortDirection)
        {
            try
            {
                var hostels = await _hostelService.GetHostelsByUserIdAsync(landlordId,searchPhrase, pageNumber, pageSize, sortBy, sortDirection);
                if (hostels.Succeeded && hostels.Data != null)
                {
                    return Ok(hostels);
                }

                return NotFound(hostels.Errors ?? new List<string> { "No hostels found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
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
                    return Ok(result);
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
                return BadRequest(new Response<HostelResponseDto>
                {
                    Succeeded = false,
                    Message = "Invalid model state."
                });
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
                var result = await _hostelService.UpdateHostelAsync(hostelId, request, imageUrls);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(new Response<HostelResponseDto>
                {
                    Succeeded = false,
                    Message = result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<HostelResponseDto>
                {
                    Succeeded = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [HttpDelete("DeleteHostel/{id}")]
        public async Task<IActionResult> DeleteHostel(Guid id)
        {
            try
            {
                var result = await _hostelService.DeleteHostelAsync(id);
                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return NotFound(result);
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

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAll([FromBody] GetAllHostelQuery request)
        {
            try
            {
                var response = await _hostelService.GetAllHostelAsync(request);
                if (!response.Succeeded || response.Data == null || !response.Data.Any())
                {
                    return NotFound(new PagedResponse<List<ListHostelResponseDto>>
                    {
                        Succeeded = false,
                        Message = "No hostels found."
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}