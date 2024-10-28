using HostelFinder.Application.DTOs.Amenity.Response;
using HostelFinder.Application.DTOs.RoomDetails.Response;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Requests;

public class PostResponseDto
{
    public Guid Id { get; set; }
    public Guid HostelId { get; set; }
    public Guid RoomId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Status { get; set; }
    public DateTime DateAvailable { get; set; }
    public Guid MembershipId { get; set; }
}