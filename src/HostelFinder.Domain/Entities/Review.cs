using RoomFinder.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelFinder.Domain.Entities
{
    public class Review : BaseEntity
    {
        public string Comment {  get; set; }
        public int rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public Guid UserId { get; set; }
        public Guid HostelId { get; set; }
        public virtual User User { get; set; }
        public virtual Hostel Hostel { get; set; }
    }
}
