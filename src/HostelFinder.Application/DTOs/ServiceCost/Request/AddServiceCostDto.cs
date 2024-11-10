namespace HostelFinder.Application.DTOs.ServiceCost.Request;

public class AddServiceCostDto
{
    public Guid RoomId { get; set; }
    public Guid ServiceId { get; set; }
    public decimal UnitCost { get; set; }
    public Guid? InVoiceId { get; set; }
}