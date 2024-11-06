using HostelFinder.Domain.Enums;
using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities
{
    public class Room : BaseEntity
    {
        public Guid HostelId { get; set; }
        public string? RoomName {  get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public int? Floor { get; set; } 
        public int MaxRenters { get; set; }
        public decimal Size { get; set; }
        public bool IsAvailable {  get; set; }
        public decimal MonthlyRentCost { get; set; }
        public decimal Deposit {  get; set; }
        public RoomType RoomType { get; set; }

        //Navigation
        public virtual Hostel Hostel { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<RoomAmenities> RoomAmenities { get; set; } = new HashSet<RoomAmenities>();
        public virtual RoomDetails RoomDetails { get; set; }
        public virtual ICollection<ServiceCost> ServiceCosts { get; set; } = new HashSet<ServiceCost>();

        public virtual ICollection<Image>? Images { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    }
}