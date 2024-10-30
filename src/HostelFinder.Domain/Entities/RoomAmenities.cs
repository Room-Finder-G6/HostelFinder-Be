using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities;

public class RoomAmenities 
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid AmenityId { get; set; }
    public virtual Amenity Amenity { get; set; }
    public virtual Room Room { get; set; }
}