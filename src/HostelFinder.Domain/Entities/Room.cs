using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities
{
    public class Room : BaseEntity
    {
        [Required]
        public Guid HostelId { get; set; }

        [Required]
        public Guid RoomTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoomName { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public Guid? BookingId { get; set; }

        public virtual Hostel? Hostel { get; set; }  
        public virtual RoomType? RoomType { get; set; } 
        public virtual Deposit? Booking { get; set; } 
    }
}
