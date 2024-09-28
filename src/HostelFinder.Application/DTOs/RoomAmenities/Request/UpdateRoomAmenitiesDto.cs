namespace HostelFinder.Application.DTOs.RoomAmenities.Request;

public class UpdateRoomAmenitiesDto
{
    public bool HasAirConditioner { get; set; }
    public bool HasElevator { get; set; } 
    public bool HasWifi { get; set; } 
    public bool HasFridge { get; set; } 
    public bool HasGarage { get; set; } 
    public bool HasFireExtinguisher { get; set; } 
    public bool HasEmergencyExit { get; set; } 
    public string? OtherAmenities { get; set; } = string.Empty;
}