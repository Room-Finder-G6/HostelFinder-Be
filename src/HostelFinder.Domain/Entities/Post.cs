using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelFinder.Domain.Entities
{
    public class Post : BaseEntity
    {
        [ForeignKey("Hostel")]
        public Guid HostelId { get; set; }
        [ForeignKey("Room")]
        public Guid RoomId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public bool Status { get; set; } = true;
        public DateTime DateAvailable { get; set; }
        public Guid MembershipServiceId { get; set; }
        public virtual Hostel Hostel { get; set; }  
        public virtual Room Room { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<WishlistPost> WishlistPosts { get; set; }
        public virtual MembershipServices MembershipServices { get; set; }
    }
}