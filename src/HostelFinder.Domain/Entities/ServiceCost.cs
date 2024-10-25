using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities;

public class ServiceCost : BaseEntity
{
    [ForeignKey("Room")] 
    [Required] 
    public Guid RoomId { get; set; }
    [Required] 
    public string ServiceName { get; set; }
    public decimal unitCost { get; set; }
    public decimal Cost { get; set; }
    public int PreviousReading {  get; set; }
    public int CurrentReading {  get; set; }
    public virtual Room Room { get; set; }
    public virtual Invoice Invoice { get; set; }
}