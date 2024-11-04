using HostelFinder.Domain.Enums;
using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class Room : BaseEntity
    {
        public Guid HostelId { get; set; }
        public string? RoomName {  get; set; }
        public float Size { get; set; }
        public bool Status {  get; set; }
        public decimal MonthlyRentCost { get; set; }
        public decimal Deposit {  get; set; }
        public RoomType RoomType { get; set; }
        public virtual Hostel Hostel { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<RoomAmenities> RoomAmenities { get; set; }
        public virtual RoomDetails RoomDetails { get; set; }
        public virtual ICollection<ServiceCost> ServiceCost { get; set; }
    }
}