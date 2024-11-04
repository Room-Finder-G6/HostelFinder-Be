using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities;

public class ServiceCost : BaseEntity
{
    [ForeignKey("Room")] 
    [Required] 
    public Guid RoomId { get; set; }
    public Guid? InVoiceId { get; set; }
    [ForeignKey("Service")]
    [Required]
    public Guid ServiceId { get; set; }
    
    public decimal UnitCost { get; set; }
    public decimal Cost { get; set; }
    public int PreviousReading {  get; set; }
    public int CurrentReading {  get; set; }

    //navigation
    public virtual Room Room { get; set; }
    public virtual Invoice Invoice { get; set; }
    public virtual Service Service { get; set; }
}