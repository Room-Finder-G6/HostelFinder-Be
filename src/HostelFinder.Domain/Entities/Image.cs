﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities;

public class Image : BaseEntity
{
    [Required] [MaxLength(255)] public string Url { get; set; }
    [ForeignKey("Hostel")] public Guid? HostelId { get; set; }
    [ForeignKey("Post")] public Guid? PostId { get; set; }
    [MaxLength(255)] public string? Description { get; set; }
    public virtual Hostel Hostel { get; set; }
    public virtual Post Post { get; set; }
}