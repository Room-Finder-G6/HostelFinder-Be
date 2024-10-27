using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.DTOs.RoomDetails.Request;
using HostelFinder.Application.DTOs.ServiceCost.Request;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Application.DTOs.Post.Requests;

public class AddPostRequestDto
{
    [Required]
    public Guid HostelId { get; set; }
    [Required]
    public Guid RoomId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    public bool Status { get; set; } = true;
    [Required]
    public DateTime DateAvailable { get; set; }
    public List<AddRoomAmenityDto> AddRoomAmenity { get; set; }
    public AddRoomDetailsDto RoomDetails { get; set; }
    public List<AddServiceCostDto> ServiceCosts { get; set; } 
    public Guid MembershipServiceId { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}