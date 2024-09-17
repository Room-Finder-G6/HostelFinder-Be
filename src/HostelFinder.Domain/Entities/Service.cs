using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class Service : BaseEntity
    {
        public string ServiceName { get; set; }
        public Guid HostelId { get; set; }
        public int Price { get; set; }
        public virtual Hostel Hostel { get; set; }
    }
}