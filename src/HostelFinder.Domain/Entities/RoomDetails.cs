﻿using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities;

public class RoomDetails
{
    [Key]
    public Guid PostId { get; set; }

    public int BedRooms { get; set; } 
    public int BathRooms { get; set; }
    public int Kitchen { get; set; }
    public decimal Size { get; set; }
    public bool Status { get; set; }
    [MaxLength(255)]
    public string? OtherDetails { get; set; }
    
    public virtual Post Post { get; set; } = default!;
}