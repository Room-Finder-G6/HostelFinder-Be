using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Application.DTOs.ServiceCost.Request;

public class AddServiceCostDto
{
    [Required] 
    public string ServiceName { get; set; }
    [Required] 
    public decimal Cost { get; set; }
}