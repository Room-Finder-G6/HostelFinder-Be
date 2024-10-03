using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelFinder.Domain.Entities
{
    public class Hostel : BaseEntity
    {
        [ForeignKey("User")]
        public Guid? LandlordId { get; set; }
        [Required]
        [MaxLength(50)]
        public string HostelName { get; set; } 
        [MaxLength(255)]
        public string? Description { get; set; } 
        [Required]
        [MaxLength(255)]
        public float? Size { get; set; }
        [Required]
        public int NumberOfRooms { get; set; }
        public string? Coordinates { get; set; }
        [Required]
        public float Rating { get; set; }
        public virtual ICollection<Service> Services { get; set; } 
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual User Landlord { get; set; } 
        public virtual Address Address { get; set; } 

    }
}