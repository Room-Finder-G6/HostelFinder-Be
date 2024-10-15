namespace HostelFinder.Domain.Entities;

public class RoomAmenities
{
    public Guid PostId { get; set; }
    public Guid AmenityId { get; set; }
    public Amenity Amenity { get; set; }
    public virtual Post Post { get; set; }
}