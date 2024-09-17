using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class RoomFeature : BaseEntity
    {
        public string Name { get; set; }
        public Guid RoomId { get; set; }
        public virtual Room? Room { get; set; }
    }
}
