using HostelFinder.Application.DTOs.Users.Requests;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User/GetListUser
        [HttpGet("GetListUser")]
        public async Task<IActionResult> GetListUser()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // PUT: api/User/UpdateUser/{userId}
        [HttpPut("UpdateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserRequestDto request)
        {
            var result = await _userService.UpdateUserAsync(userId, request);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Data);
        }

        // PUT: api/User/UnActiveUser/{userId}
        [HttpPut("UnActiveUser/{userId}")]
        public async Task<IActionResult> UnActiveUser(Guid userId)
        {
            var result = await _userService.UnActiveUserAsync(userId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Data);
        }
    }
}
