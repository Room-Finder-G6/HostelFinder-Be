using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelFinder.Domain.Entities
{
    public class Wishlist : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<WishlistRoom> WishlistRooms { get; set; }
    }
}
