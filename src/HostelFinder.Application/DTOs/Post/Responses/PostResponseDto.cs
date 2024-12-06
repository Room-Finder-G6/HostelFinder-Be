using HostelFinder.Application.DTOs.Address;

namespace HostelFinder.Application.DTOs.Post.Responses;

public class PostResponseDto
{
    public Guid Id { get; set; }
    public Guid HostelId { get; set; }
    public Guid RoomId { get; set; }
    public AddressDto Address { get; set; }
    public Guid WishlistPostId { get; set; }
    public string Title { get; set; }
    public decimal Size { get; set; }
    public string Description { get; set; }
    public string ImageUrls { get; set; }
    public bool Status { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public Guid MembershipServiceId { get; set; }
}