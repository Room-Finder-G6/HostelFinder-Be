using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelFinder.Domain.Entities
{
    public class WishlistRoom
    {
        public Guid WishlistId { get; set; }
        public Wishlist Wishlist { get; set; }

        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }
}
