using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities;

public class ServiceCost : BaseEntity
{
    [ForeignKey("Room")] 
    [Required] 
    public Guid RoomId { get; set; }
    public Guid? InvoiceId { get; set; }
    [ForeignKey("Service")]
    [Required]
    public Guid ServiceId { get; set; }
    [Required]
    [Column(TypeName ="decimal(18,2)")]
    
    public decimal UnitCost { get; set; }


    //navigation
    public virtual Room Room { get; set; }  
    public virtual Service Service { get; set; }
}