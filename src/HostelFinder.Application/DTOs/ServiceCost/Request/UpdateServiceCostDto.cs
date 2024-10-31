namespace HostelFinder.Application.DTOs.ServiceCost.Request;

public class UpdateServiceCostDto
{
    public Guid ServiceCostId { get; set; }
    public string ServiceName { get; set; }
    public decimal UnitCost { get; set; }
    public decimal Cost { get; set; }
    public int PreviousReading { get; set; }
    public int CurrentReading { get; set; }
    public Guid? InVoiceId { get; set; }
}