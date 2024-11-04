using System.ComponentModel.DataAnnotations.Schema;
using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class Service : BaseEntity
    {
        public string ServiceName { get; set; }
        public bool IsCharge { get; set; }
        public ICollection<HostelServices> HostelServices { get; set; }
    }
}