using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Application.DTOs.ServiceCost.Request;

public class AddServiceCostDto
{
    public Guid PostId { get; set; }
    [Required] 
    public string ServiceName { get; set; }
    [Required] 
    public decimal Cost { get; set; }
}