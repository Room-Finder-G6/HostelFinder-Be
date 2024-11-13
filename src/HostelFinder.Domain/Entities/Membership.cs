using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class Membership : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public virtual ICollection<MembershipServices> MembershipServices { get; set; }
        public virtual ICollection<UserMembership> UserMemberships { get; set; }
    }
}
