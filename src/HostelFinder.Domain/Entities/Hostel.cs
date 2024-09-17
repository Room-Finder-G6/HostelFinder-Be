using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities
{
    public class Hostel : BaseEntity
    {
        [Required]
        public Guid LandlordId { get; set; }

        [Required]
        [MaxLength(100)]
        public string HostelName { get; set; } 

        [MaxLength(500)]
        public string? Description { get; set; } 

        [Required]
        [MaxLength(256)]
        public string Address { get; set; } 

        public float? Size { get; set; }

        [Required]
        public int NumberOfRooms { get; set; }

        public string? Coordinates { get; set; }

        public string? Images { get; set; } 

        public float OverallRating { get; set; }

        public Guid ServiceId { get; set; }


       
        public virtual Service Service { get; set; } 
        public virtual User Landlord { get; set; } 
    }
}