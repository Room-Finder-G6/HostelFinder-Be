using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class Membership_Services : BaseEntity
    {
        public string Service_Name {  get; set; }
        public Guid MembershipId { get; set; }
        public virtual Membership Membership { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}