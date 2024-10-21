using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class Membership : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public virtual ICollection<Membership_Services> Membership_Services { get; set; }
        public virtual ICollection<UserMembership> UserMemberships { get; set; }
    }
}
