using HostelFinder.Application.DTOs.Membership.Requests;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpGet("GetListMembership")]
        public async Task<IActionResult> GetListMembership()
        {
            var response = await _membershipService.GetAllMembershipWithMembershipService();
            if (!response.Succeeded || response.Data == null)
            {
                return NotFound(response.Errors);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("AddMembership")]
        public async Task<IActionResult> AddMembership([FromForm] AddMembershipRequestDto membershipDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _membershipService.AddMembershipAsync(membershipDto);
                if (response.Data != null)
                {
                    return Ok(response);
                }

                return BadRequest(response.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        [HttpPut("EditMembership/{id}")]
        public async Task<IActionResult> EditMembership(Guid id, [FromForm] UpdateMembershipRequestDto membershipDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _membershipService.EditMembershipAsync(id, membershipDto);
                if (response.Succeeded)
                {
                    return Ok(response);
                }

                return BadRequest(response.Message);
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

        [HttpDelete("DeleteMembership/{id}")]
        public async Task<IActionResult> DeleteMembership(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new Response<string>
                    {
                        Succeeded = false,
                        Message = "Invalid membership ID."
                    });
                }

                var response = await _membershipService.DeleteMembershipAsync(id);
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

        [HttpPost("AddUserMembership")]
        public async Task<IActionResult> AddUserMembership([FromForm] AddUserMembershipRequestDto userMembershipDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<string>
                {
                    Succeeded = false,
                    Message = "Invalid request model."
                });
            }

            try
            {
                var response = await _membershipService.AddUserMembershipAsync(userMembershipDto);
                if (response.Succeeded)
                {
                    return Ok(response);
                }

                return BadRequest(response);
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

        //[HttpGet("GetMembershipByUserId")]
        //public async 
    }
}
