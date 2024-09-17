using RoomFinder.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelFinder.Domain.Entities
{
    public class BookingRequest : BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public bool Status {  get; set; }
        public DateTime DateRequest { get; set; }

        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
