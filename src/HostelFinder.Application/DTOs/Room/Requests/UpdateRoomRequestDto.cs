using HostelFinder.Application.DTOs.RoomAmenities.Request;
using HostelFinder.Application.DTOs.RoomDetails.Request;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Requests;

public class UpdateRoomRequestDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string PrimaryImageUrl { get; set; }
    public virtual RoomType RoomType { get; set; } 
    public decimal? Size { get; set; }
    public decimal MonthlyRentCost { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime DateAvailable { get; set; }
    public UpdateRoomAmenitiesDto RoomAmenities { get; set; }
    public UpdateRoomDetailsDto RoomDetails { get; set; }
}