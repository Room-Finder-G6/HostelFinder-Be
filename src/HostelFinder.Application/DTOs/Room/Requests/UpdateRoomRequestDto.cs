using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Requests;

public class UpdateRoomRequestDto
{
    public Guid HostelId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public virtual RoomType RoomType { get; set; } 
    public decimal? Size { get; set; }
    public decimal MonthlyRentCost { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime DateAvailable { get; set; }

}