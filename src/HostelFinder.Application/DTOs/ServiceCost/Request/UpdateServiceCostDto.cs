using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelFinder.Application.DTOs.ServiceCost.Request;

public class UpdateServiceCostDto
{
    public Guid? ServiceCostId { get; set; } // Nullable for new entries
    [Required]
    public string ServiceName { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Cost { get; set; }
}