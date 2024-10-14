using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.Wishlist.Request;
using HostelFinder.Application.DTOs.Wishlist.Response;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class WishlistService : IWishlistService
    {

        private readonly IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<Response<bool>> AddRoomToWishlistAsync(AddRoomToWishlistRequestDto request)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(request.UserId);
            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    UserId = request.UserId,
                    WishlistRooms = new List<WishlistRoom>()
                };
                await _wishlistRepository.AddAsync(wishlist);
            }

            var wishlistRoom = new WishlistRoom
            {
                WishlistId = wishlist.Id,
                RoomId = request.RoomId
            };

            await _wishlistRepository.AddRoomToWishlistAsync(wishlistRoom);

            return new Response<bool>(true);
        }

        public async Task<Response<WishlistResponseDto>> GetWishlistByUserIdAsync(Guid userId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            if (wishlist == null)
            {
                return new Response<WishlistResponseDto>(null, "Wishlist not found");
            }

            var response = new WishlistResponseDto
            {
                WishlistId = wishlist.Id,
                Rooms = wishlist.WishlistRooms.Select(wr => new RoomResponseDto
                {
                    Id = wr.Room.Id,
                    Title = wr.Room.Title,
                }).ToList()
            };
            return new Response<WishlistResponseDto>(response);
        }

        public async Task<Response<bool>> DeleteRoomFromWishlistAsync(Guid id)
        {
            var wishlist = await _wishlistRepository.GetByIdAsync(id);
            if (wishlist == null)
            {
                return new Response<bool>(false, "Wishlist not found");
            }

            await _wishlistRepository.DeletePermanentAsync(wishlist.Id);
            return new Response<bool>(true);
        }

    }
}
