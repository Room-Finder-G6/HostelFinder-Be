﻿using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Domain.Entities
{
    public class Room : BaseEntity
    {
        [ForeignKey("Hostel")]
        public Guid HostelId { get; set; }
        [ForeignKey("RoomType")]
        public Guid RoomTypeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        public decimal? Size { get; set; }
        public decimal MonthlyRentCost { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime DateAvailable { get; set; }
        public virtual Hostel Hostel { get; set; }  
        public virtual RoomType RoomType { get; set; } 
        public virtual ICollection<BookingRequest> BookingRequests { get; set; }
        public virtual RoomDetails RoomDetails { get; set; }
        public virtual RoomAmenities RoomAmenities { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<ServiceCost> ServiceCosts { get; set; }
    }
}
