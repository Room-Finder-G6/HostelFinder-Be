using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class UserMembership : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int PostsUsed { get; set; }
        public Guid MembershipId { get; set; }
        public Membership Membership { get; set; }
    }
}
