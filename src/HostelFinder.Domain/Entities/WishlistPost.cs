﻿
namespace HostelFinder.Domain.Entities
{
    public class WishlistPost
    {
        public Guid WishlistId { get; set; }
        public Wishlist Wishlist { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}