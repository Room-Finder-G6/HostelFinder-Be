namespace HostelFinder.Domain.Entities;

public class RoomAmenities
{
    public Guid RoomId { get; set; }
    public Room Room { get; set; }
    public Guid AmenityId { get; set; }
    public Amenity Amenity { get; set; }
}