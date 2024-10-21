using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelFinder.Domain.Entities
{
    public class UserMembership
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid MembershipId { get; set; }
        public Membership Membership { get; set; }
    }
}
