using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities;

public class RoomDetails : BaseEntity
{
    [Key]
    public Guid PostId { get; set; }
    public int BedRooms { get; set; } 
    public int BathRooms { get; set; }
    public int Kitchen { get; set; }
    public int Size { get; set; }
    public virtual Room Room { get; set; } = default!;
}