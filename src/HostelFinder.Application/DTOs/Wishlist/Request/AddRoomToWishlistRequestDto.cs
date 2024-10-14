namespace HostelFinder.Application.DTOs.Wishlist.Request
{
    public class AddRoomToWishlistRequestDto
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
    }
}
