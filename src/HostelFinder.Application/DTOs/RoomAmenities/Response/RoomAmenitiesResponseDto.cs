namespace HostelFinder.Application.DTOs.RoomAmenities.Response;

public class RoomAmenitiesResponseDto
{
    public bool HasAirConditioner { get; set; } = false;
    public bool HasElevator { get; set; } = false;
    public bool HasWifi { get; set; } = false;
    public bool HasFridge { get; set; } = false;
    public bool HasGarage { get; set; } = false;
    public bool HasFireExtinguisher { get; set; } = false;
    public bool HasEmergencyExit { get; set; } = false;
    public string? OtherAmenities { get; set; } = string.Empty;
}