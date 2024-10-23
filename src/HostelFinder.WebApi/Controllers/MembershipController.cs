using HostelFinder.Application.DTOs.Membership.Requests;
using HostelFinder.Application.Interfaces.IServices;
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
            if (response.Succeeded)
            {
                return Ok(response);
            }

            return NotFound(response.Errors);
        }

        [HttpPost]
        [Route("AddMembership")]
        public async Task<IActionResult> AddMembership([FromBody] AddMembershipRequestDto membershipDto)
        {
            var response = await _membershipService.AddMembershipAsync(membershipDto);
            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response.Errors);
        }

        [HttpPut("EditMembership/{id}")]
        public async Task<IActionResult> EditMembership(Guid id, [FromBody] UpdateMembershipRequestDto membershipDto)
        {
            var response = await _membershipService.EditMembershipAsync(id, membershipDto);
            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response.Errors);
        }

        [HttpDelete("DeleteMembership/{id}")]
        public async Task<IActionResult> DeleteMembership(Guid id)
        {
            var response = await _membershipService.DeleteMembershipAsync(id);
            if (response.Succeeded)
            {
                return Ok(response);
            }

            return BadRequest(response.Errors);
        }
    }
}
