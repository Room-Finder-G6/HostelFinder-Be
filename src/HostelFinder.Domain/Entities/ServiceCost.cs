using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities;

public class ServiceCost : BaseEntity
{
    [ForeignKey("Post")] [Required] 
    public Guid PostId { get; set; }
    [Required] 
    public string ServiceName { get; set; }
    [Required] 
    public decimal Cost { get; set; }
    public virtual Post Post { get; set; }
}