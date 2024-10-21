using HostelFinder.Application.DTOs.Wishlist.Request;
using HostelFinder.Application.DTOs.Wishlist.Response;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IWishlistService
    {
        Task<Response<bool>> AddRoomToWishlistAsync(AddRoomToWishlistRequestDto request);
        Task<Response<WishlistResponseDto>> GetWishlistByUserIdAsync(Guid userId);
        Task<Response<bool>> DeleteRoomFromWishlistAsync(Guid id);
    }
}
