using System.ComponentModel.DataAnnotations;
namespace HostelFinder.Application.DTOs.ServiceCost.Request;

public class UpdateServiceCostDto
{
    [Required]
    public string ServiceName { get; set; }
    public decimal unitCost { get; set; }
    public int PreviousReading { get; set; }
    public int CurrentReading { get; set; }
    public decimal Cost { get; set; }
}