﻿using System;
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

        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
