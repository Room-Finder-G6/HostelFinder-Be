using System.ComponentModel.DataAnnotations;
using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class RoomFeature : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }
        public Guid RoomId { get; set; }
        public virtual Room? Room { get; set; }
    }
}
