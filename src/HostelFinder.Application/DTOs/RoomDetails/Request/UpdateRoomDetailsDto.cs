using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Application.DTOs.RoomDetails.Request;

public class UpdateRoomDetailsDto
{
    public int BedRooms { get; set; } 
    public int BathRooms { get; set; }
    public int Kitchen { get; set; }
    public decimal Size { get; set; }
    public bool Status { get; set; }
    [MaxLength(255)]
    public string? OtherDetails { get; set; }
}