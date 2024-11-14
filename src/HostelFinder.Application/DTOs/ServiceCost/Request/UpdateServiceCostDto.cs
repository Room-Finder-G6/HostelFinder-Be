using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Application.DTOs.ServiceCost.Request;

public class UpdateServiceCostDto
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "UnitCost must be non-negative.")]
    public decimal UnitCost { get; set; }

    public DateTime? EffectiveTo { get; set; }
}