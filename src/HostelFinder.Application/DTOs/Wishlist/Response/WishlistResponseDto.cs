using HostelFinder.Application.DTOs.Room.Requests;

namespace HostelFinder.Application.DTOs.Wishlist.Response
{
    public class WishlistResponseDto
    {
        public Guid WishlistId { get; set; }
        public List<RoomResponseDto> Rooms { get; set; }
    }
}
