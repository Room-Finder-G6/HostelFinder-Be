using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities;

public class RoomDetails
{
    [Key]
    [ForeignKey("Room")]
    public Guid RoomId { get; set; }

    public int BedRooms { get; set; } 
    public int BathRooms { get; set; }
    public int Kitchen { get; set; }
    public decimal Size { get; set; }
    public bool Status { get; set; }
    [MaxLength(255)]
    public string? OtherDetails { get; set; } = string.Empty;
    
    public virtual Room Room { get; set; } = default!;
}