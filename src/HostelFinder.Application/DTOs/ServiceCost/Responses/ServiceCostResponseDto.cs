namespace HostelFinder.Application.DTOs.ServiceCost.Responses;

public class ServiceCostResponseDto
{
    public string ServiceName { get; set; }
    public decimal Cost { get; set; }
    public decimal unitCost { get; set; }
    public int PreviousReading { get; set; }
    public int CurrentReading { get; set; }

}