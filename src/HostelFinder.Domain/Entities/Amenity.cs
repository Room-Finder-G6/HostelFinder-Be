﻿using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities;

public class Amenity : BaseEntity
{
    public string AmenityName { get; set; }
    public bool IsSelected { get; set; }
    public virtual ICollection<RoomAmenities> RoomAmenities { get; set; }
}