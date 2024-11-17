using HostelFinder.Application.DTOs.Users.Requests;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/users")]
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
            try
            {
                var users = await _userService.GetAllUsersAsync();
                if (!users.Succeeded)
                {
                    return NotFound(users);
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }

        }

        // PUT: api/User/UpdateUser/{userId}
        [HttpPut("UpdateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserRequestDto request)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(userId, request);
                if (!result.Succeeded)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong!");
            }

        }

        // PUT: api/User/UnActiveUser/{userId}
        [HttpPut("UnActiveUser/{userId}")]
        public async Task<IActionResult> UnActiveUser(Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.UnActiveUserAsync(userId);
            if (!result.Succeeded)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(user);
            }
            return Ok(user);
        }
        
        [HttpGet("GetUserByHostelId/{hostelId}")]
        public async Task<IActionResult> GetUserByHostelId(Guid hostelId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userService.GetUserByHostelIdAsync(hostelId);
            if (user == null)
            {
                return NotFound(user);
            }
            return Ok(user);
        }
    }
}