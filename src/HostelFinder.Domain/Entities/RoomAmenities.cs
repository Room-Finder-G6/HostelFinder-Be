using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelFinder.Domain.Entities;

public class RoomAmenities
{
    [Key]
    [ForeignKey("Room")]
    public Guid RoomId { get; set; }

    public bool HasAirConditioner { get; set; }
    public bool HasElevator { get; set; } 
    public bool HasWifi { get; set; } 
    public bool HasFridge { get; set; } 
    public bool HasGarage { get; set; } 
    public bool HasFireExtinguisher { get; set; } 
    public bool HasEmergencyExit { get; set; }
    [MaxLength(255)]
    public string? OtherAmenities { get; set; } = string.Empty;

    public virtual Room Room { get; set; } = default!;
}