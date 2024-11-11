using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.DTOs.RoomDetails.Request;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Requests;

public class UpdatePostRequestDto
{
    public Guid HostelId { get; set; }
    public Guid RoomId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Status { get; set; } = true;
    public DateTime DateAvailable { get; set; }
}