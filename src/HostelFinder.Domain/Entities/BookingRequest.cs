﻿using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Domain.Entities
{
    public class BookingRequest : BaseEntity
    {
        [Key]
        public Guid RequestId { get; set; }
        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        [Required]
        public DateTime DateRequest { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
