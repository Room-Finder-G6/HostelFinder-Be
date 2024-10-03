using HostelFinder.Application.DTOs.Wishlist.Request;
using HostelFinder.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {

        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        // POST: api/Wishlist/AddRoomToWishlist
        [HttpPost("AddRoomToWishlist")]
        public async Task<IActionResult> AddRoomToWishlist([FromBody] AddRoomToWishlistRequestDto request)
        {
            var result = await _wishlistService.AddRoomToWishlistAsync(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
        }

        // GET: api/Wishlist/GetWishlistByUserId/{userId}
        [HttpGet("GetWishlistByUserId/{userId}")]
        public async Task<IActionResult> GetWishlistByUserId(Guid userId)
        {
            var result = await _wishlistService.GetWishlistByUserIdAsync(userId);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return NotFound(result.Errors);
        }

        // DELETE: api/Wishlist/DeleteRoomFromWishlist
        [HttpDelete("DeleteRoomFromWishlist")]
        public async Task<IActionResult> DeleteWishlist(Guid id)
        {
            var result = await _wishlistService.DeleteRoomFromWishlistAsync(id);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
        }

    }
}
