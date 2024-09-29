using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelFinder.Domain.Entities;

public class RoomAmenities
{
    [Key]
    public Guid RoomId { get; set; }

    public bool HasAirConditioner { get; set; } = false;
    public bool HasElevator { get; set; } = false;
    public bool HasWifi { get; set; } = false;
    public bool HasFridge { get; set; } = false;
    public bool HasGarage { get; set; } = false;
    public bool HasFireExtinguisher { get; set; } = false;
    public bool HasEmergencyExit { get; set; } = false;
    [MaxLength(255)] public string? OtherAmenities { get; set; }

    public virtual Room Room { get; set; } = default!;
}